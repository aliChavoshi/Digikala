using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Classes;
using Digikala.Core.Interfaces.AdminPanel;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.DtosAndViewModels.AdminPanel.Category;
using Digikala.Utility.Generator;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

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

        #region Parent
        public IActionResult CreateParent()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateParent(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            if (await _categoryRepository.IsExist(x => x.Name == model.Name))
            {
                return RedirectToAction("Index");
            }
            var category = ObjectMapper.Mapper.Map<CreateCategoryViewModel, Category>(model);
            category.CreatorUser = User.GetUserId();

            await _categoryRepository.Add(category);
            await _categoryRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(CategoryParamsDto paramsDto)
        {
            ViewBag.FilterRoot = paramsDto.FilterRoot;
            ViewBag.FilterTitle = paramsDto.FilterTitle;
            return View(await _categoryRepository.GetParentCategories(paramsDto));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> EditParent(int id)
        {
            var category = await _categoryRepository.GetById(id);
            return PartialView(ObjectMapper.Mapper.Map<Category, EditCategoryViewModel>(category));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditParent(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var category = await _categoryRepository.GetById(model.Id);
            if (category.Version != model.Version)
            {
                return RedirectToAction("Index");
            }
            if (category.Name != model.Name)
            {
                if (await _categoryRepository.IsExist(x => x.Name.ToLower() == model.Name.ToLower()))
                {
                    return RedirectToAction("Index");
                }
            }
            var result = ObjectMapper.Mapper.Map(model, category);
            result.ModifierUser = User.GetUserId();
            _categoryRepository.Update(result);
            await _categoryRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Index");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            return PartialView(await _categoryRepository.GetById(id));
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> DeleteParent(Category model)
        {
            var category = await _categoryRepository.GetById(model.Id);
            if (await _categoryRepository.IsExist(x => x.ParentId == model.Id))
            {
                return RedirectToAction("Index");
            }
            await _categoryRepository.DeleteCategory(category, User.GetUserId());
            TempData["IsSuccess"] = true;
            return RedirectToAction("Index");
        }
        #endregion

        #region SubCategory

        [HttpGet("{id:int}")]
        public async Task<IActionResult> SubCategories(int id)
        {
            ViewBag.rootId = id;
            return View(await _categoryRepository.ToListAsync());
        }

        [HttpGet("{id:int}/{rootId:int}")]
        public async Task<IActionResult> CreateSubCategory(int id, int rootId)
        {
            var category = await _categoryRepository.GetById(id);
            var model = ObjectMapper.Mapper.Map<Category, CreateSubCategoryDto>(category);
            model.RootId = rootId;
            return PartialView(model);
        }

        [HttpPost("{id:int}/{rootId:int}")]
        public async Task<IActionResult> CreateSubCategory(CreateSubCategoryDto model)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryRepository.IsExist(x => x.Name == model.Name))
                {
                    return RedirectToAction("SubCategories", new { id = model.RootId });
                }
                var subCategory = ObjectMapper.Mapper.Map<CreateSubCategoryDto, Category>(model);
                subCategory.CreatorUser = User.GetUserId();
                await _categoryRepository.Add(subCategory);
                await _categoryRepository.Save();
                TempData["IsSuccess"] = true;
            }
            return RedirectToAction("SubCategories", new { id = model.RootId });
        }

        [HttpGet("{id:int}/{rootId:int}")]
        public async Task<IActionResult> EditSubCategory(int id, int rootId)
        {
            var category = await _categoryRepository.GetById(id);
            var model = ObjectMapper.Mapper.Map<Category, EditSubCategoryDto>(category);
            model.RootId = rootId;
            return PartialView(model);
        }

        [HttpPost("{id:int}/{rootId:int}")]
        public async Task<IActionResult> EditSubCategory(EditSubCategoryDto model)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetById(model.Id);
                if (category.Version != model.Version)
                {
                    return RedirectToAction("SubCategories", new { id = model.RootId });
                }
                if (category.Name != model.Name && await _categoryRepository.IsExist(x => x.Name == model.Name))
                {
                    return RedirectToAction("SubCategories", new { id = model.RootId });
                }
                var result = ObjectMapper.Mapper.Map(model, category);
                result.ModifierUser = User.GetUserId();
                _categoryRepository.Update(result);
                await _categoryRepository.Save();
                TempData["IsSuccess"] = true;
            }
            return RedirectToAction("SubCategories", new { id = model.RootId });
        }

        [HttpGet("{id:int}/{rootId:int}")]
        public async Task<IActionResult> DeleteSubCategory(int id, int rootId)
        {
            ViewBag.rootId = rootId;
            return PartialView(await _categoryRepository.GetById(id));
        }

        [HttpPost("{id:int}/{rootId:int}")]
        public async Task<IActionResult> DeleteSubCategory(Category model, int id, int rootId)
        {
            var category = await _categoryRepository.GetById(model.Id);
            if (await _categoryRepository.IsExist(x => x.ParentId == model.Id))
            {
                return RedirectToAction("SubCategories", new { id = rootId });
            }
            await _categoryRepository.DeleteCategory(category, User.GetUserId());
            TempData["IsSuccess"] = true;
            return RedirectToAction("SubCategories", new { id = rootId });
        }
        #endregion
    }
}
