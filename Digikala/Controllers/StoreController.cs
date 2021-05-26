using AutoMapper;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.Store;
using Microsoft.AspNetCore.Mvc;
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
                ModelState.AddModelError("Mobile", $"با این شماره همراه {model.Mobile}در سایت ثبت نام نکرده اید");
                return View(model);
            }

            var user = await _accountRepository.GetUserByMobile(model.Mobile);
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
                //TODO If Email is Valid Then Confirm Email
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
            var user = await _accountRepository.GetUser(model.Mobile, model.Password);
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
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
