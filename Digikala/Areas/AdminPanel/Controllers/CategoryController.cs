using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces.AdminPanel;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.InputParams.AdminPanel.Category;
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

        public async Task<IActionResult> Create()
        {
            await CategoriesForSelectList();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (await _categoryRepository.IsExist(x => x.Name == model.Name))
            {
                return RedirectToAction("Index");
            }
            var category = ObjectMapper.Mapper.Map<Category>(model);
            category.CreatorUser = User.GetUserId();
            await _categoryRepository.Add(category);
            await _categoryRepository.Save();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(CategoryParamsDto paramsDto)
        {
            ViewBag.FilterRoot = paramsDto.FilterRoot;
            ViewBag.FilterTitle = paramsDto.FilterTitle;
            return View(await _categoryRepository.CategoriesToList(paramsDto));
        }
    }
}
