using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Services
{
    public class RolePermissionService : GenericRepository<RolePermission>,IRolePermissionService
    {
        private readonly DigikalaContext _context;

        public RolePermissionService(DigikalaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsRoleHavePermission(int permissionId, int roleId)
        {
            return await IsExist(x => x.RoleId == roleId && x.PermissionId == permissionId);
        }
    }
}