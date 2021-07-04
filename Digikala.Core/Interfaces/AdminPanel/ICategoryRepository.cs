using System.Collections.Generic;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Category;
using Digikala.DTOs.FormGenericDto.Public;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Core.Interfaces.AdminPanel
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<SelectListItem>> CategoriesForSelectList();
        Task<GetAllGenericByPaginationDto<Category>> CategoriesToList(CategoryParamsDto paramsDto);
        Task DeleteCategory(Category category,int userId);
    }
}