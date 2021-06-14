using System.Threading.Tasks;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.Core.Interfaces.Identity;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Digikala.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Route("[area]/[controller]/[action]")]
    [Permission(2)]
    public class HomeController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public HomeController(IRoleRepository roleRepository, IAccountRepository accountRepository, IRolePermissionRepository rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Role



        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            return View(await _roleRepository.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(Role model)
        {
            if (!ModelState.IsValid)
            {
                //TODO
                return RedirectToAction("Roles");
            }
            if (await _roleRepository.IsExist(x => x.Title.ToLower() == model.Title.ToLower()))
            {
                //TODO
                return RedirectToAction("Roles");
            }
            await _roleRepository.Add(model);
            await _roleRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Roles");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> EditRole(int id)
        {
            return PartialView(await _roleRepository.GetById(id));
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> EditRole(Role model)
        {
            var role = await _roleRepository.GetById(model.Id);
            if (model.Title != role.Title)
            {
                if (await _roleRepository.IsExist(x => x.Title.ToLower() == model.Title.ToLower()))
                {
                    //TODO
                    return RedirectToAction("Roles");
                }
            }
            role.Title = model.Title;
            _roleRepository.Update(role);
            await _roleRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Roles");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            return PartialView(await _roleRepository.GetById(id));
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> DeleteRole(Role model)
        {
            if (await _accountRepository.IsExist(x => x.RoleId == model.Id))
            {
                //TODO
                return RedirectToAction("Roles");
            }
            if (await _rolePermissionRepository.IsExist(x => x.RoleId == model.Id))
            {
                //TODO
                return RedirectToAction("Roles");
            }
            var role = await _roleRepository.GetById(model.Id);
            role.IsDeleted = true;
            _roleRepository.Update(role);
            await _roleRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Roles");
        }

        #endregion

        #region Permission

        

        #endregion
    }
}
