using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Digikala.Core.Services.Identity
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DigikalaContext context) : base(context)
        {
        }

        public async Task<List<SelectListItem>> PermissionsForSelectList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "لطفا انتخاب کنید",
                    Value = "0"
                }
            };

            var permissions = await Context.Permission.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.AddRange(permissions);
            return list;
        }
    }
}