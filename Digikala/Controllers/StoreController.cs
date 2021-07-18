using AutoMapper;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.Store;
using Digikala.Utility.Convertor;
using Digikala.Utility.Generator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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

        private async Task LoginUserClaim(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone,user.Mobile),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await HttpContext.SignInAsync(principal, properties);
        }
        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(StoreRegisterDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            //کاربر از قبل ثبت نام کرده است
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
                    ModelState.AddModelError(nameof(model.Mobile), "شما قبلا ثبت نام کرده اید لطفا وارد بخش فروشندگان شوید");
                    return View(model);
                }

                #region CheckingEmail
                //کاربر ایمیل از قبل نداشته است
                if (string.IsNullOrEmpty(user.Email))
                {
                    //اگر کاربر از قبل ایمیل نداشته باشد،نباید ایمیلی که الان وارد کرده
                    //در دیتابیس موجود باشد
                    if (await _accountRepository.IsExistMail(model.Email))
                    {
                        ModelState.AddModelError(nameof(model.Email), "ایمیل وارد شده تکراری میباشد لطفا آن را بررسی کنید");
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
                else if (user.Email.ToLower() != model.Email.Trim().ToLower())
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده با ایمیلی که قبلا وارد کرده اید متفاوت است ");
                    return View(model);
                }

                #endregion

                var store = _mapper.Map<StoreRegisterDto, Store>(source: model);
                store.UserId = user.Id;

                //update user to store roleId
                await _storeRepository.Add(store);
                //نقش کاربر را باید تغییر بدهیم
                await _accountRepository.UpdateSaveUserRoleId(user, 2);

                TempData["IsSuccess"] = true;
                //الان کاربر اطلاعاتش را از قبل پر کرده بوده است 
                //پس باید به لاگین انتقال داده شود
                return RedirectToAction("Login");
            }
            //کاربر از قبل یوزر نداشته باشد
            else
            {
                //اگر کاربر ثبت نام نکرده است پس نباید ایمیل آن هم در دیتابیس باشد
                if (await _accountRepository.IsExistMail(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "ایمیل وارد شده تکراری میباشد لطفا ایمیل را بررسی کنید");
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
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> LoginAsync(StoreLoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _accountRepository.IsExistUserByMobileEmailPass(model.Mobile, model.Email,
                model.Password);
            if (user != null)
            {
                if (user.IsActive)
                {
                    if (await _storeRepository.IsExistUser(user.Id))
                    {
                        #region LognInWeb
                        await LoginUserClaim(user);
                        #endregion

                        TempData["LoginSuccess"] = true;
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(nameof(model.Email), "فروشگاهی برای شما یافت نشد ابتدا آن را بسازید");
                    return View(model);
                }
                ModelState.AddModelError(nameof(model.Email), "حساب کاربری شما غیر فعال میباشد");
                return View(model);
            }
            ModelState.AddModelError(nameof(model.Mobile), "کاربری با مشخصات وارد شده یافت نشد");
            return View(model);
        }
        #endregion

        #region Index
        [HttpGet]
        [Permission(1)]
        public async Task<IActionResult> Index()
        {
            if (!await _storeRepository.IsExistUser(User.GetUserId()))
            {
                return RedirectToAction("Login", "Account",
                    new { permission = false, returnUrl = "/store/index" });
            }
            return View();
        }
        #endregion

        #region Properties

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
                return View(model);
            if (userId != model.UserId)
                return NotFound();
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
