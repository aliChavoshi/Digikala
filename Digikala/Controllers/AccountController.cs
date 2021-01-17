using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.Core.Utility;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.AccountDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Digikala.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public AccountController(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        private async Task SendSms(string mobile, string activeCode)
        {
            try
            {
                var messageSender = new MessageSender();
                await messageSender.Sms(mobile, "کد فعال سازی : " + activeCode);
            }
            catch
            {
                //
            }
        }
        private async Task LoginUserClaim(User user, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone,user.Mobile)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };
            await HttpContext.SignInAsync(principal, properties);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "قبلا با  این شماره تلفن ثبت نام کرده اید.");
                return View(model);
            }

            var user = _mapper.Map<RegisterDto, User>(source: model);
            await _accountRepository.AddUser(user);

            #region SendSms
            await SendSms(user.Mobile, user.ActiveCode);
            #endregion

            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ActivateUser", new ActivateDto { Mobile = user.Mobile });
        }

        [HttpGet("{mobile}")]
        public IActionResult ActivateUser(string mobile)
        {
            return View(new ActivateDto { Mobile = mobile });
        }

        [HttpPost("{mobile}")]
        public async Task<IActionResult> ActivateUser(string mobile, ActivateDto model)
        {
            if (mobile != model.Mobile)
            {
                return NotFound();
            }
            if (!await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل مورد نظر یافت نشد.");
                return View(model);
            }
            var mergeCode = CodeGenerators.MergeCodes(model.ActiveCode);

            var user = await _accountRepository.GetUser(model.Mobile);
            if (user.ActiveCode != mergeCode)
            {
                ModelState.AddModelError("ActiveCode", "کد وارد شده اشتباه است لطفا مجدد تلاش کنید .");
                return View(model);
            }
            if (user.IsActive && user.ConfirmMobile)
            {
                return RedirectToAction("ResetPassword", new RegisterDto { Mobile = model.Mobile });
            }
            else
            {
                if (await _accountRepository.ActivateUser(mergeCode, user.Mobile))
                    return RedirectToAction("Login");
            }

            return View(model);
        }

        public async Task<IActionResult> ResendActiveCode(string mobile)
        {
            var user = await _accountRepository.GetUser(mobile);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _accountRepository.ResetActiveCode(user);

            #region SendSms
            await SendSms(mobile, result.ActiveCode);
            #endregion

            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ActivateUser", new ActivateDto { Mobile = mobile });
        }

        [HttpGet]
        public async Task<IActionResult> ChangeMobileNumber(string mobile)
        {
            var user = await _accountRepository.GetUser(mobile);
            if (user == null)
            {
                return NotFound();
            }
            return View(new ChangeMobileNumberDto { OldMobile = user.Mobile });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeMobileNumber(ChangeMobileNumberDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _accountRepository.IsExistMobileNumber(model.NewMobile))
            {
                ModelState.AddModelError("NewMobile", "قبلا با  این شماره تلفن ثبت نام کرده اید.");
                return View(model);
            }
            var user = await _accountRepository.GetUser(model.OldMobile);
            if (user == null)
            {
                return NotFound();
            }

            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            var result = await _accountRepository.ChangeMobileNumberOfUser(user, model.NewMobile);

            #region SendSms
            await SendSms(result.Mobile, result.ActiveCode);
            #endregion

            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ActivateUser", new ActivateDto { Mobile = result.Mobile });
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var hashPass = HashGenerators.Encrypt(model.Password);
            var user = await _accountRepository.GetUser(model.Mobile, hashPass);
            if (user != null)
            {
                if (user.IsActive)
                {
                    #region LognInWeb

                    await LoginUserClaim(user, model.RememberMe);
                    #endregion

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        //TODO
                        TempData["LoginSuccess"] = true;
                        return Redirect("/");
                    }
                }
                else
                {
                    ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال میباشد");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("Mobile", "کاربری با مشخصات وارد شده یافت نشد");
                return View(model);
            }

            //TODO
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل وارد شده اشتباه است.");
                return View(model);
            }

            return await ResendActiveCode(model.Mobile);
        }

        public IActionResult ResetPassword(string mobile)
        {
            return View(new RegisterDto() { Mobile = mobile });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string mobile, RegisterDto model)
        {
            if (mobile != model.Mobile)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _accountRepository.GetUser(mobile);
            if (user == null)
            {
                return NotFound();
            }

            var hash = HashGenerators.Encrypt(model.Password);
            user.Password = hash;
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            await _accountRepository.UpdateUser(user);

            return RedirectToAction("Login");
        }
    }
}
