using System.Collections.Generic;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.FormDto.Public;
using Digikala.DTOs.InputParams.AdminPanel.Home;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Core.Interfaces.AdminPanel.Identity
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<GetAllGenericByPaginationDto<Role>> RolesToList(RoleParamsDto paramsDto);
        Task<List<SelectListItem>> RolesForSelectList();
    }
}