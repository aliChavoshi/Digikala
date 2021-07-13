using AutoMapper;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.Store;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Digikala.Utility.Convertor;
using Digikala.Utility.Generator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace Digikala.Controllers
{
    [Route("[controller]/[action]")]
    public class StoreController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISaveFileDirectory _saveFileDirectory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IViewRenderService _viewRenderService;

        public StoreController(IAccountRepository accountRepository, IStoreRepository storeRepository, IMapper mapper, IConfiguration configuration, IViewRenderService viewRenderService, ISaveFileDirectory saveFileDirectory)
        {
            _accountRepository = accountRepository;
            _storeRepository = storeRepository;
            _mapper = mapper;
            _configuration = configuration;
            _viewRenderService = viewRenderService;
            _saveFileDirectory = saveFileDirectory;
        }

        #region Properties
        private async Task SendSms(string mobile, string activeCode)
        {
            var messageSender = new MessageSender(_configuration, _viewRenderService);
            await messageSender.Sms(mobile, "کد فعال سازی : " + activeCode);
        }
        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(StoreRegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                var user = await _accountRepository.GetUser(model.Mobile, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.Mobile), "شماره همراه یا رمز عبور اشتباه است ");
                    return View(model);
                }
                if (await _storeRepository.IsExistUser(user.Id))
                {
                    ModelState.AddModelError("Mobile", "شما قبلا ثبت نام کرده اید لطفا وارد بخش فروشندگان شوید");
                    return View(model);
                }

                #region CheckingEmail
                //کاربر ایمیل از قبل نداشته است
                if (string.IsNullOrEmpty(user.Email))
                {
                    if (await _accountRepository.IsExistMail(model.Email))
                    {
                        ModelState.AddModelError("Email", "ایمیل وارد شده تکراری میباشد");
                        return View(model);
                    }
                    user.Email = model.Email;
                    user.ActiveCodeEmail = CodeGenerators.GuidId();

                    #region SendActiveEmailCode

                    var messageSender = new MessageSender(_configuration, _viewRenderService);
                    await messageSender.SendMailToUserWithView("فعال سازی ایمیل ", user,
                        "Account/_PartialActiveEmail", "/store/index");

                    #endregion
                }
                //ایمیلی که الان میزند با ایمیلی که قبلا زده متفاوت است
                else if (user.Email.ToLower() != model.Email.ToLower())
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده با ایمیلی که قبلا وارد کرده اید متفاوت است ");
                    return View(model);
                }

                #endregion

                var store = _mapper.Map<StoreRegisterDto, Store>(source: model);
                store.UserId = user.Id;

                //update user to store roleId
                await _storeRepository.Add(store);
                await _accountRepository.UpdateSaveUserRoleId(user, 2);

                TempData["IsSuccess"] = true;
                return View();
            }
            else
            {
                if (await _accountRepository.IsExistMail(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "ایمیل وارد شده با شماره همراه همخوانی ندارد");
                    return View(model);
                }
                var user = _mapper.Map<StoreRegisterDto, User>(model);
                await _accountRepository.Add(user);
                await _accountRepository.Save();

                var store = _mapper.Map<StoreRegisterDto, Store>(source: model);
                store.UserId = user.Id;

                await _storeRepository.Add(store);
                await _storeRepository.Save();

                #region SendActiveEmailCode

                var messageSender = new MessageSender(_configuration, _viewRenderService);
                await messageSender.SendMailToUserWithView("فعال سازی ایمیل ", user,
                    "Account/_PartialActiveEmail", "/store/index");

                #endregion
                #region SendSms
                await SendSms(user.Mobile, user.ActiveCode);
                #endregion

                TempData["ResendActiveCode"] = true;
                return RedirectToAction("ConfirmMobileNumber", "Account",
                    new { mobile = model.Mobile, returnUrl = "/store/index" });
            }
        }

        #endregion

        #region ConfirmMobileAndEmail

        [Permission(1)]
        public async Task<IActionResult> ConfirmMobileNumber()
        {
            var user = await _accountRepository.GetUserByMobile(User.GetMobileNumber());
            if (user.ConfirmMobile)
            {
                TempData["IsConfirmed"] = true;
                return RedirectToAction("Index");
            }
            #region SendSms
            await SendSms(user.Mobile, user.ActiveCode);
            #endregion

            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ConfirmMobileNumber", "Account",
                new { mobile = User.GetMobileNumber(), returnUrl = "/store/index" });
        }

        [Permission(1)]
        public async Task<IActionResult> ConfirmEmail()
        {
            var user = await _accountRepository.GetUserByMobile(User.GetMobileNumber());
            if (user.ConfirmEmail)
            {
                TempData["IsConfirmed"] = true;
                return RedirectToAction("Index");
            }

            #region SendActiveEmailCode

            var messageSender = new MessageSender(_configuration, _viewRenderService);
            await messageSender.SendMailToUserWithView("فعال سازی ایمیل ", user,
                "Account/_PartialActiveEmail", "/store/index");

            #endregion

            TempData["ResendConfirmEmail"] = true;
            return RedirectToAction("Index");
        }

        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login(bool permission = false, string backUrl = "")
        {
            return RedirectToAction("Login", "Account",
                new { permission = permission, returnUrl = backUrl });
        }

        #endregion

        #region Index

        [HttpGet]
        [Permission(1)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> Properties(int userId)
        {
            var store = await _storeRepository.GetStoreByUserId(userId);
            return View(ObjectMapper.Mapper.Map<Store, PropertiesStoreDto>(store));
        }

        [HttpPost("{userId:int}")]
        public async Task<IActionResult> Properties(int userId, PropertiesStoreDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (userId != model.UserId)
            {
                return NotFound();
            }
            var store = await _storeRepository.GetStoreByUserId(model.UserId);
            var result = ObjectMapper.Mapper.Map(model, store);
            if (string.IsNullOrEmpty(model.OldLogo))
            {
                result.Logo = await _saveFileDirectory.SaveFile(model.Logo,
                    _configuration["PathSaveFileDirectory:StoreLogo"]);
            }
            else
            {
                result.Logo = await _saveFileDirectory.DeleteAndSaveFile(model.OldLogo, model.Logo,
                    _configuration["PathSaveFileDirectory:StoreLogo"]);
            }
            _storeRepository.Update(result);
            await _storeRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Index");
        }
        #endregion
    }
}
