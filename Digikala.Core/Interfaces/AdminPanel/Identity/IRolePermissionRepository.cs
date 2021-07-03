using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Home;
using Digikala.DTOs.FormGenericDto.Public;

namespace Digikala.Core.Interfaces.AdminPanel.Identity
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<bool> IsRoleHavePermission(int permissionId, int roleId);
        Task AddPermissionsIdToRole(CreateRolePermissionDto model);
        Task<GetAllGenericByPaginationDto<RolePermission>> RolePermissionToList(RolePermissionParamsDto paramsDto);
        Task UpdateSaveRolePermission(RolePermission model,string expireDate="");
    }
}