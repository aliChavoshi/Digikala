using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.Core.Interfaces.Identity;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Digikala.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Route("[area]/[controller]/[action]")]
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

        #region Role

        public IActionResult Index()
        {
            return View();
        }

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
                return RedirectToAction("Roles");
            }
            if (await _roleRepository.IsExist(x => x.Title.ToLower() == model.Title.ToLower()))
            {
                return RedirectToAction("Roles");
            }
            await _roleRepository.Add(model);
            await _roleRepository.Save();
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
                    return RedirectToAction("Roles");
                }
            }
            role.Title = model.Title;
            _roleRepository.Update(role);
            await _roleRepository.Save();
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
                return RedirectToAction("Roles");
            }
            if (await _rolePermissionRepository.IsExist(x => x.RoleId == model.Id))
            {
                return RedirectToAction("Roles");
            }
            var role = await _roleRepository.GetById(model.Id);
            role.IsDeleted = true;
            _roleRepository.Update(role);
            await _roleRepository.Save();
            return RedirectToAction("Roles");
        }

        #endregion
    }
}
