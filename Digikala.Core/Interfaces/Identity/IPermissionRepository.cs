using System.Collections.Generic;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.FormDto.Public;
using Digikala.DTOs.InputParams.AdminPanel.Home;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Core.Interfaces.Identity
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<SelectListItem>> PermissionsForSelectList();
        Task<GetAllGenericByPaginationDto<Permission>> PermissionsToList(PermissionParamsDto paramsDto);
    }
}