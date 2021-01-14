using System.Threading.Tasks;
using Digikala.Core.Interfaces;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.Core.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolePermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsRoleHavePermission(int permissionId, int roleId)
        {
            return await _unitOfWork.Repository<RolePermission>()
                .IsExist(x => x.RoleId == roleId && x.PermissionId == permissionId);
        }
    }
}