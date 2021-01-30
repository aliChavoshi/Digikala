using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.Core.Utility;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.AccountDtos;
using Digikala.DTOs.Store;
using Ghasedak.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;

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
                ModelState.AddModelError("Mobile", "شماره موبایل یافت نشد ابتدا در سایت ثبت نام کنید یا شماره را بررسی کنید");
                return View(model);
            }

            var hashPass = HashGenerators.Encrypt(model.Password);
            var user = await _accountRepository.GetUser(model.Mobile, hashPass);
            if (user == null)
            {
                ModelState.AddModelError("Password", "لطفا شماره همراه یا کلمه عبور خود را بررسی کنید");
                return View(model);
            }
            if (await _storeRepository.IsExistUser(user.Id))
            {
                ModelState.AddModelError("Mobile", "شما قبلا ثبت نام کرده اید لطفا وارد بخش فروشندگان شوید");
                return View(model);
            }
            //شاید قبلا ایمیل خودش را وارد کرده باشد 
            //اگر ایمیلی وجود نداشت باید ایمیل را ثبت کنیم
            if (string.IsNullOrEmpty(user.Email))
            {
                if (await _accountRepository.IsExistMail(model.Email))
                {
                    ModelState.AddModelError("Email", "قبلا از این ایمیل استفاده شده است لطفا ایمیل را بررسی کنید.");
                    return View(model);
                }
                //TODO CONFIRM EMAIL
                //TODO If Emial is Valid Then Confirm Email
                user.Email = model.Email;
                await _accountRepository.UpdateUser(user);
            }
            else if (user.Email != model.Email)
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده با ایمیلی که قبلا وارد کرده اید همخوانی ندارد ");
                return View(model);
            }

            var store = _mapper.Map<StoreRegisterDto, Store>(source: model);
            store.UserId = user.Id;

            await _storeRepository.Insert(store);
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
            var hashPass = HashGenerators.Encrypt(model.Password);
            var user = await _accountRepository.GetUser(model.Mobile, hashPass);
            if (user != null)
            {
                if (user.IsActive)
                {
                    if (!await _storeRepository.IsExistUser(user.Id))
                    {
                        ModelState.AddModelError("Mobile", "چنین کاربری هم اکنون در فروشنگان یافت نشد ، لطفا ابتدا در بخش فروشندگان ثبت نام کنید");
                        return View(model);
                    }
                    if (!await _storeRepository.IsActiveStore(user.Id))
                    {
                        ModelState.AddModelError("Mobile", "فروشگاه شما هنوز فعال نشده است با مدیر سایت ارتباط برقرار کنید");
                        return View(model);
                    }
                    //TODO Index TempData Swal
                    TempData["LoginSuccess"] = true;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال میباشد");
                return View(model);
            }
            ModelState.AddModelError("Mobile", "کاربری با مشخصات وارد شده یافت نشد");
            return View(model);
        }

        [HttpGet]
        [Permission(5)]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
