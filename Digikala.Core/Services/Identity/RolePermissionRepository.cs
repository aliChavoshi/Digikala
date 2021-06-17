using System.Threading.Tasks;
using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Services.Identity
{
    public class RolePermissionRepository : GenericRepository<RolePermission>,IRolePermissionRepository
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

        public async Task AddPermissionsIdToRole(int roleId, int[] permissionIds, string[] dateExpire)
        {
            throw new System.NotImplementedException();
        }
    }
}