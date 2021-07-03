using System.Collections.Generic;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Home;
using Digikala.DTOs.FormGenericDto.Public;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Core.Interfaces.AdminPanel.Identity
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<SelectListItem>> PermissionsForSelectList();
        Task<GetAllGenericByPaginationDto<Permission>> PermissionsToList(PermissionParamsDto paramsDto);
        Task DeleteSave(Permission permission);
    }
}