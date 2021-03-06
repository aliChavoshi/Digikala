﻿using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.AdminPanel.Identity;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Home;
using Digikala.Utility.Convertor;

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
        private async Task RolesForSelectList(int id = 0)
        {
            var roles = await _roleRepository.RolesForSelectList();
            ViewData["Roles"] = new SelectList(roles, "Value", "Text", id);
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
        public async Task<IActionResult> Roles(RoleParamsDto paramsDto)
        {
            ViewBag.FilterTitle = paramsDto.FilterTitle;

            return View(await _roleRepository.RolesToList(paramsDto));
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

        #region SubPermission
        [HttpGet("{id:int}")]
        public async Task<IActionResult> SubPermissions(int id)
        {
            ViewBag.rootId = id;
            return View(await _permissionRepository.ToListAsync());
        }

        [HttpGet("{id:int}/{rootId:int}")]
        public async Task<IActionResult> CreateSubPermission(int id, int rootId)
        {
            var category = await _permissionRepository.GetById(id);
            var model = ObjectMapper.Mapper.Map<Permission, CreateSubPermissionDto>(category);
            model.RootId = rootId;
            return PartialView(model);
        }

        [HttpPost("{id:int}/{rootId:int}")]
        public async Task<IActionResult> CreateSubPermission(CreateSubPermissionDto model)
        {
            if (ModelState.IsValid)
            {
                if (await _permissionRepository.IsExist(x => x.Name == model.Name))
                {
                    return RedirectToAction("SubPermissions", new { id = model.RootId });
                }

                var permission = ObjectMapper.Mapper.Map<CreateSubPermissionDto, Permission>(model);
                await _permissionRepository.Add(permission);
                await _permissionRepository.Save();
                TempData["IsSuccess"] = true;
            }
            return RedirectToAction("SubPermissions", new { id = model.RootId });
        }

        [HttpGet("{id:int}/{rootId:int}")]
        public async Task<IActionResult> EditSubPermission(int id, int rootId)
        {
            var category = await _permissionRepository.GetById(id);
            var model = ObjectMapper.Mapper.Map<Permission, EditSubPermissionDto>(category);
            model.RootId = rootId;
            return PartialView(model);
        }

        [HttpPost("{id:int}/{rootId:int}")]
        public async Task<IActionResult> EditSubPermission(EditSubPermissionDto model)
        {
            if (ModelState.IsValid)
            {
                var permission = await _permissionRepository.GetById(model.Id);
                if (permission.Name != model.Name && await _permissionRepository.IsExist(x => x.Name == model.Name))
                {
                    return RedirectToAction("SubPermissions", new { id = model.RootId });
                }
                var result = ObjectMapper.Mapper.Map(model, permission);
                _permissionRepository.Update(result);
                await _permissionRepository.Save();
                TempData["IsSuccess"] = true;
            }
            return RedirectToAction("SubPermissions", new { id = model.RootId });

        }

        [HttpGet("{id:int}/{rootId:int}")]
        public async Task<IActionResult> DeleteSubPermission(int id, int rootId)
        {
            ViewBag.rootId = rootId;
            return PartialView(await _permissionRepository.GetById(id));
        }

        [HttpPost("{id:int}/{rootId:int}")]
        public async Task<IActionResult> DeleteSubPermission(Permission model,int id, int rootId)
        {
            if (await _rolePermissionRepository.IsExist(x => x.PermissionId == model.Id))
            {
                return RedirectToAction("SubPermissions", new { id = rootId });
            }
            if (await _permissionRepository.IsExist(x => x.ParentId == model.Id))
            {
                return RedirectToAction("SubPermissions", new { id = rootId });
            }
            var permission = await _permissionRepository.GetById(model.Id);
            await _permissionRepository.DeleteSave(permission);
            TempData["IsSuccess"] = true;
            return RedirectToAction("SubPermissions", new { id = rootId });
        }
        #endregion

        #region ParentPermission

        public async Task<IActionResult> Permissions(PermissionParamsDto paramsDto)
        {
            ViewBag.FilterTitle = paramsDto.FilterTitle;
            return View(await _permissionRepository.ParentPermissionsToList(paramsDto));
        }

        public IActionResult CreatePermission()
        {
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
            permission.Name = model.Name;
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
            await _permissionRepository.DeleteSave(permission);
            TempData["IsSuccess"] = true;
            return RedirectToAction("Permissions");
        }
        #endregion

        #region RolePermission

        [HttpGet]
        public async Task<IActionResult> CreateRolePermission()
        {
            await RolesForSelectList();
            await PermissionsForSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRolePermission(CreateRolePermissionDto model)
        {
            await _rolePermissionRepository.AddPermissionsIdToRole(model);
            TempData["IsSuccess"] = true;
            return RedirectToAction("RolePermissions");
        }

        [HttpGet]
        public async Task<IActionResult> RolePermissions(RolePermissionParamsDto paramsDto)
        {
            ViewBag.FilterPermission = paramsDto.FilterPermission;
            ViewBag.FilterRole = paramsDto.FilterRole;
            return View(await _rolePermissionRepository.RolePermissionToList(paramsDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> EditRolePermission(int id)
        {
            var rolePermission = await _rolePermissionRepository.GetById(id);
            await RolesForSelectList(rolePermission.RoleId);
            await PermissionsForSelectList(rolePermission.PermissionId);
            return PartialView(rolePermission);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditRolePermission(RolePermission model, string expireDate = "")
        {
            var rolePer = await _rolePermissionRepository.GetById(model.Id);
            await _rolePermissionRepository.UpdateSaveRolePermission(rolePer, expireDate);
            TempData["IsSuccess"] = true;
            return RedirectToAction("RolePermissions");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteRolePermission(int id)
        {
            var rolePermission = await _rolePermissionRepository.GetById(id);
            await RolesForSelectList(rolePermission.RoleId);
            await PermissionsForSelectList(rolePermission.PermissionId);
            return PartialView(rolePermission);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteRolePermission(RolePermission model)
        {
            var rolePer = await _rolePermissionRepository.GetById(model.Id);
            _rolePermissionRepository.Delete(rolePer);
            await _rolePermissionRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("RolePermissions");
        }
        #endregion
    }
}
