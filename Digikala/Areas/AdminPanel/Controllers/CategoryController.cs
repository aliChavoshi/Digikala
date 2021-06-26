using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.AdminPanel;
using Digikala.DataAccessLayer.Entities.Store;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Digikala.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Route("[area]/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #region Properties

        private async Task CategoriesForSelectList(int id = 0)
        {
            var permissions = await _categoryRepository.CategoriesForSelectList();
            ViewData["Categories"] = new SelectList(permissions, "Value", "Text", id);
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            await CategoriesForSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            await CategoriesForSelectList(model.ParentId ?? 0);
            return View();
        }
    }
}
