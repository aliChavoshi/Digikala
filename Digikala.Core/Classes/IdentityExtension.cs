using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Digikala.Core.Classes
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
        public static string GetMobileNumber(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return Convert.ToInt32(claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        }
    }
}