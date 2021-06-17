using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Interfaces.Identity
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<bool> IsRoleHavePermission(int permissionId, int roleId);
        Task AddPermissionsIdToRole(int roleId, int[] permissionIds, string[] dateExpire);
    }
}