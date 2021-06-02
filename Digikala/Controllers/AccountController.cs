using System;
using AutoMapper;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.AccountDtos;
using Digikala.Utility.Generator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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

        #region Properties
        private static async Task SendSms(string mobile, string activeCode)
        {
            var messageSender = new MessageSender();
            await messageSender.Sms(mobile, "کد فعال سازی : " + activeCode);
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
        private async Task ResendActiveCode(string mobile)
        {
            var user = await _accountRepository.GetUserByMobile(mobile);

            var result = await _accountRepository.ResetActiveCodeUpdateSaveUser(user);

            #region SendSms
            await SendSms(mobile, result.ActiveCode);
            #endregion
        }
        #endregion

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                ModelState.AddModelError("Mobile", "قبلا با  این شماره تلفن ثبت نام کرده اید.");
                return View(model);
            }

            var user = _mapper.Map<RegisterDto, User>(source: model);
            await _accountRepository.Add(user);
            await _accountRepository.Save();

            #region SendSms
            await SendSms(user.Mobile, user.ActiveCode);
            #endregion

            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ConfirmMobileNumber", new { mobile = user.Mobile });
        }

        #endregion

        #region ConfirmMobileNumber

        [HttpGet]
        public IActionResult ConfirmMobileNumber(string mobile)
        {
            return View(new ConfirmMobileDto { Mobile = mobile });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmMobileNumber(ConfirmMobileDto model, bool resendCode = false)
        {
            if (resendCode)
            {
                await ResendActiveCode(model.Mobile);
                TempData["ResendActiveCode"] = true;
                ModelState.Clear();
                return View(model);
            }
            if (!await _accountRepository.IsExistMobileNumber(model.Mobile))
            {
                ModelState.AddModelError("Mobile", $"با این شماره {model.Mobile}  قبلا ثبت نام کرده اید");
                return View(model);
            }

            var mergeActiveCode = CodeGenerators.MergeIntArray(model.ActiveCode);
            var user = await _accountRepository.GetUserByMobile(model.Mobile);
            if (user.ActiveCode != mergeActiveCode)
            {
                ModelState.AddModelError("ActiveCode", "کد تایید وارد شده اشتباه است لطفا مجدد تلاش کنید .");
                return View(model);
            }

            await _accountRepository.ConfirmMobileAndActiveUserUpdateSaveUser(user);
            TempData["SuccessConfirmMobile"] = true;
            return RedirectToAction("Login");
        }

        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login(bool permission = true, string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            ViewBag.permission = permission;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _accountRepository.GetUser(model.Mobile, model.Password);
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
                        //TODO For Alert
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
            return View(model);
        }

        #endregion

        #region ChangeMobileNumber

        [HttpGet]
        public async Task<IActionResult> ChangeMobileNumber(string mobile)
        {
            var user = await _accountRepository.GetUserByMobile(mobile);
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
            if (model.NewMobile == model.OldMobile)
            {
                ModelState.AddModelError("NewMobile", $"شماره همراه وارد شده با شماره همراه قبلی یکسان است ");
                return View(model);
            }
            if (await _accountRepository.IsExistMobileNumber(model.NewMobile))
            {
                ModelState.AddModelError("NewMobile", $"با این شماره همراه {model.NewMobile}  قبلا در دیجی کالا ثبت نام کرده اید");
                return View(model);
            }
            var user = await _accountRepository.GetUserByMobile(model.OldMobile);
            if (user == null)
            {
                ModelState.AddModelError("OldMobile", $"کاربری با این مشخصات یافت نشد");
                return View(model);
            }
            //new active code for user
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            var result = await _accountRepository.ChangeMobileNumberOfUserUpdateSaveUser(user, model.NewMobile);

            #region SendSms
            await SendSms(result.Mobile, result.ActiveCode);
            #endregion

            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ConfirmMobileNumber", new { mobile = result.Mobile });
        }

        #endregion

        #region ForgotPassword

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
                ModelState.AddModelError("Mobile", $"شماره وارد شده {model.Mobile} در سیستم موجود نمیباشد ");
                return View(model);
            }

            await ResendActiveCode(model.Mobile);
            TempData["ResendActiveCode"] = true;
            return RedirectToAction("ResetPassword", new { mobile = model.Mobile });
        }

        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string mobile)
        {
            return View(new ResetPasswordDto { Mobile = mobile });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model, bool resendCode = false)
        {
            if (resendCode)
            {
                await ResendActiveCode(model.Mobile);
                TempData["ResendActiveCode"] = true;
                ModelState.Clear();
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _accountRepository.GetUserByMobile(model.Mobile);
            if (user.ActiveCode != model.ActiveCode)
            {
                ModelState.AddModelError("ActiveCode", "کد فعالسازی وارد شده اشتباه است لطفا مجدد تلاش کنید ");
                return View(model);
            }

            user.Password = HashGenerators.Encrypt(model.Password);
            user.ActiveCode = CodeGenerators.ActiveCodeFiveNumbers();
            _accountRepository.Update(user);
            await _accountRepository.Save();

            TempData["SuccessResetPassword"] = true;
            return RedirectToAction("Login");
        }

        #endregion

        #region Logout

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion
    }
}
