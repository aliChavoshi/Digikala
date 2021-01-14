using System.Threading.Tasks;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Interfaces
{
    public interface IRolePermissionService 
    {
        Task<bool> IsRoleHavePermission(int permissionId, int roleId);
    }
}