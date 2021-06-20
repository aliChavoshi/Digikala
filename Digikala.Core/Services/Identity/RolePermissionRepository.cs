using System.Collections.Generic;
using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.InputParams.AdminPanel.Home;
using Digikala.Utility.Convertor;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Digikala.Core.Services.Identity
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly DigikalaContext _context;

        public RolePermissionRepository(DigikalaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsRoleHavePermission(int permissionId, int roleId)
        {
            return await IsExist(x => x.RoleId == roleId && x.PermissionId == permissionId);
        }

        public async Task AddPermissionsIdToRole(CreateRolePermissionDto model)
        {
            var perIdForRoleIdDb = await Context.RolePermission
                .Where(x => x.RoleId == model.RoleId).Select(x => x.PermissionId).ToListAsync();

            #region Distinct

            //distinct permission id & expire date 
            var listPermission = new List<int>();
            var listExpDate = new List<string>();
            for (int i = 0; i < model.PermissionId.Count; i++)
            {
                if (perIdForRoleIdDb.Any(x => x == model.PermissionId[i]))
                {
                    continue;
                }
                listPermission.Add(model.PermissionId[i]);
                listExpDate.Add(model.ExpireRolePermission[i]);
            }

            #endregion

            if (listPermission.Any())
            {
                await _context.RolePermission
                    .AddRangeAsync(listPermission.Select((permissionId, index) =>
                        new RolePermission
                        {
                            PermissionId = permissionId,
                            ExpireRolePermission = listExpDate[index] == "0" ? null : listExpDate[index].ToMiladi(),
                            RoleId = model.RoleId
                        }));
                await _context.SaveChangesAsync();
            }
        }
    }
}