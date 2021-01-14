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
        private IUserService _userService;
        private IRolePermissionService _rolePermissionService;
        public PermissionAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));
            _rolePermissionService = (IRolePermissionService)context.HttpContext.RequestServices.GetService(typeof(IRolePermissionService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = context.HttpContext.User.GetUserId();
                var userRoleId = _userService.GetUserRole(userId).Result;
                if (!_rolePermissionService.IsRoleHavePermission(_permissionId, userRoleId).Result)
                {
                    context.Result = new RedirectResult("/Account/Login?returnUrl=" + context.HttpContext.Request.Path);
                }
            }
            else
            {
                context.Result = new RedirectResult("/Account/Login?returnUrl=" + context.HttpContext.Request.Path);
            }
        }
    }
}