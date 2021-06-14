using System.Threading.Tasks;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.Core.Interfaces.Identity;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IPermissionRepository _permissionRepository;

        public HomeController(IRoleRepository roleRepository, IAccountRepository accountRepository, IRolePermissionRepository rolePermissionRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
        }

        #region Properties

        private async Task PermissionsForSelectList(int id = 0)
        {
            var permissions = await _permissionRepository.PermissionsForSelectList();
            ViewData["Permissions"] = new SelectList(permissions, "Value", "Text", id);
        }

        #endregion

        #region AdminPanel

        public IActionResult Index()
        {
            return View();
        }

        #endregion

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

        public async Task<IActionResult> Permissions()
        {
            return View(await _permissionRepository.ToListAsync());
        }

        public async Task<IActionResult> CreatePermission()
        {
            await PermissionsForSelectList();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission(Permission model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Permissions");
            }
            if (await _permissionRepository.IsExist(x => x.Name.ToLower() == model.Name.ToLower()))
            {
                return RedirectToAction("Permissions");
            }

            await _permissionRepository.Add(model);
            await _permissionRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Permissions");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> EditPermission(int id)
        {
            var permission = await _permissionRepository.GetById(id);
            await PermissionsForSelectList(permission.ParentId ?? 0);
            return PartialView(permission);
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> EditPermission(Permission model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Permissions");
            }
            var permission = await _permissionRepository.GetById(model.Id);
            if (permission.Name != model.Name)
            {
                if (await _permissionRepository.IsExist(x => x.Name.ToLower() == model.Name.ToLower()))
                {
                    return RedirectToAction("Permissions");
                }
            }
            if (model.ParentId.HasValue && model.ParentId.Value == permission.Id)
            {
                return RedirectToAction("Permissions");
            }
            permission.Name = model.Name;
            permission.ParentId = model.ParentId;
            _permissionRepository.Update(permission);
            await _permissionRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Permissions");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            return PartialView(await _permissionRepository.GetById(id));
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> DeletePermission(Permission model)
        {
            if (await _rolePermissionRepository.IsExist(x => x.PermissionId == model.Id))
            {
                return RedirectToAction("Permissions");
            }
            if (await _permissionRepository.IsExist(x => x.ParentId == model.Id))
            {
                return RedirectToAction("Permissions");
            }
            var permission = await _permissionRepository.GetById(model.Id);
            permission.IsDeleted = true;
            _permissionRepository.Update(permission);
            await _permissionRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Permissions");
        }
        #endregion
    }
}
