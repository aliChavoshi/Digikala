using AutoMapper;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using Digikala.Core.Classes;

namespace Digikala.Controllers
{
    [Route("[controller]/[action]")]
    public class StoreController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IMapper _mapper;

        public StoreController(IAccountRepository accountRepository, IStoreRepository storeRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _storeRepository = storeRepository;
            _mapper = mapper;
        }

        #region Properties
        private static async Task SendSms(string mobile, string activeCode)
        {
            var messageSender = new MessageSender();
            await messageSender.Sms(mobile, "کد فعال سازی : " + activeCode);
        }
        private async Task LoginUserClaim(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.MobilePhone,user.Mobile),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
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
                    //TODO CONFIRM EMAIL SEND EMAIL
                    //TODO IsActive Store with EMail
                    user.Email = model.Email;
                }
                //ایمیلی که الان میزند با ایمیلی که قبلا زده متفاوت است
                else if (user.Email != model.Email)
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
                //TODO CONFIRM EMAIL
                #region SendSms
                await SendSms(user.Mobile, user.ActiveCode);
                #endregion

                TempData["ResendActiveCode"] = true;
                return RedirectToAction("ConfirmMobileNumber", "Account", new { mobile = model.Mobile });
            }
        }

        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login(bool permission = false,string backUrl = "")
        {
            return RedirectToAction("Login", "Account", new { permission = permission, returnUrl = backUrl });
        }

        #endregion

        #region Index

        [HttpGet]
        [Permission(1)]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        #endregion
    }
}
