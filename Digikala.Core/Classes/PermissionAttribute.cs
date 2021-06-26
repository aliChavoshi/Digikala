using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.Core.Interfaces.AdminPanel.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Digikala.Core.Classes
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _permissionId;
        private IAccountRepository _accountRepository;
        private IRolePermissionRepository _rolePermissionService;
        public PermissionAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _accountRepository = (IAccountRepository)context.HttpContext.RequestServices.GetService(typeof(IAccountRepository));
            _rolePermissionService = (IRolePermissionRepository)context.HttpContext.RequestServices.GetService(typeof(IRolePermissionRepository));
            if (_accountRepository != null && _rolePermissionService != null)
            {
                if (context.HttpContext.User.Identity is { IsAuthenticated: true })
                {
                    var userId = context.HttpContext.User.GetUserId();
                    var userRoleId =  _accountRepository.GetUserRole(userId).Result;

                    if (!_rolePermissionService.IsRoleHavePermission(_permissionId, userRoleId).Result)
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
}