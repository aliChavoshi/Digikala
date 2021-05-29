﻿using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digikala.Core.Classes
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _permissionId;
        private IAccountRepository _accountRepository;
        private IRolePermissionService _rolePermissionService;
        public PermissionAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            _accountRepository = (IAccountRepository)context.HttpContext.RequestServices.GetService(typeof(IAccountRepository));
            _rolePermissionService = (IRolePermissionService)context.HttpContext.RequestServices.GetService(typeof(IRolePermissionService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetUserId();
                var userRoleId = await _accountRepository.GetUserRole(userId);
                if (!await _rolePermissionService.IsRoleHavePermission(_permissionId, userRoleId))
                {
                    context.Result = new RedirectResult("/Account/Login?permission=false&returnUrl=" + context.HttpContext.Request.Path);
                }
            }
            else
            {
                context.Result = new RedirectResult("/Account/Login?permission=false&returnUrl=" + context.HttpContext.Request.Path);
            }
        }
    }
}