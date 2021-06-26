using System.Collections.Generic;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DataAccessLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Core.Interfaces.AdminPanel
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<SelectListItem>> CategoriesForSelectList();
    }
}