using AutoMapper;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        private async Task LoginUserClaim(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.MobilePhone,user.Mobile)
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

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(StoreRegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                ModelState.AddModelError("Mobile", $"شما با این شماره {model.Mobile}  در سایت ثبت نام نکرده اید");
                return View(model);
            }
            var user = await _accountRepository.GetUserByMobile(model.Mobile);
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
            user.RoleId = 3;
            await _storeRepository.Add(store);
            await _accountRepository.UpdateUser(user);

            TempData["IsSuccess"] = true;
            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginStoreDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _accountRepository.GetUserByEmail(model.Email, model.Password);
            if (user != null)
            {
                if (user.IsActive)
                {
                    if (!await _storeRepository.IsExistUser(user.Id))
                    {
                        ModelState.AddModelError(nameof(model.Email), "شما هنوز در بخش فروشندگان ثبت نام نکرده اید");
                        return View(model);
                    }
                    if (!await _storeRepository.IsActiveStore(user.Id))
                    {
                        ModelState.AddModelError(nameof(model.Email), "فروشگاه شما هنوز فعال نشده است با مدیر سایت ارتباط برقرار کنید");
                        return View(model);
                    }

                    await LoginUserClaim(user);
                    TempData["LoginSuccess"] = true;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(nameof(model.Email), "حساب کاربری شما غیر فعال میباشد");
                return View(model);
            }
            ModelState.AddModelError(nameof(model.Email), "کاربری با مشخصات وارد شده یافت نشد");
            return View(model);
        }

        [HttpGet]
        [Permission(1)]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
