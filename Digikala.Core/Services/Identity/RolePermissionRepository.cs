using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.InputParams.AdminPanel.Home;
using Digikala.Utility.Convertor;
using System.Linq;
using System.Threading.Tasks;

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
            await _context.RolePermission
                .AddRangeAsync(model.IsExistExpireTime.Select((permissionId, index) =>
                new RolePermission()
                {
                    // PermissionId = model.,
                    //ExpireRolePermission = model.IsExistExpireTime[index] ? model.ExpireRolePermission[index].ToMiladi() : null,
                    RoleId = model.RoleId
                }));
            await _context.SaveChangesAsync();
        }
    }
}