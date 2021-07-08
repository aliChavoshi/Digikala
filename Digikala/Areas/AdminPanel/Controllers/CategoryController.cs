﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly ISaveFileDirectory _saveFileDirectory;
        private readonly IConfiguration _configuration;

        public CategoryController(ICategoryRepository categoryRepository, ISaveFileDirectory saveFileDirectory, IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _saveFileDirectory = saveFileDirectory;
            _configuration = configuration;
        }

        #region Properties

        private async Task CategoriesForSelectList(int id = 0)
        {
            var permissions = await _categoryRepository.CategoriesForSelectList();
            ViewData["Categories"] = new SelectList(permissions, "Value", "Text", id);
        }

        #endregion

        public IActionResult CreateParent()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateParent(CreateCategoryViewModel model)
        {
            if (await _categoryRepository.IsExist(x => x.Name == model.Name))
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var category = ObjectMapper.Mapper.Map<CreateCategoryViewModel, Category>(model);

            category.Icon = await _saveFileDirectory.SaveFile(model.Icon, _configuration["PathSaveFileDirectory:CategoryIcon"]);
            category.CreatorUser = User.GetUserId();

            await _categoryRepository.Add(category);
            await _categoryRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Index");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> CreateSubCategory(int id)
        {
            var category = await _categoryRepository.GetById(id);
            return PartialView(ObjectMapper.Mapper.Map<Category, CreateSubCategoryDto>(category));
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> CreateSubCategory(CreateSubCategoryDto model, int id)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryRepository.IsExist(x => x.Name == model.Name))
                {
                    return RedirectToAction("Index");
                }
                var subCategory = ObjectMapper.Mapper.Map<CreateSubCategoryDto, Category>(model);
                subCategory.CreatorUser = User.GetUserId();
                await _categoryRepository.Add(subCategory);
                await _categoryRepository.Save();
                TempData["IsSuccess"] = true;
            }
            return RedirectToAction("SubCategories", new { id = model.Id });
        }

        public async Task<IActionResult> Index(CategoryParamsDto paramsDto)
        {
            ViewBag.FilterRoot = paramsDto.FilterRoot;
            ViewBag.FilterTitle = paramsDto.FilterTitle;
            return View(await _categoryRepository.GetParentCategories(paramsDto));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> SubCategories(int id)
        {
            ViewBag.rootId = id;
            return View(await _categoryRepository.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> EditParent(int id)
        {
            var category = await _categoryRepository.GetById(id);
            var model = ObjectMapper.Mapper.Map<Category, EditCategoryViewModel>(category);
            //await CategoriesForSelectList(category.ParentId ?? 0);
            return PartialView(model);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditParent(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var category = await _categoryRepository.GetById(model.Id);
            if (category.Name != model.Name)
            {
                if (await _categoryRepository.IsExist(x => x.Name.ToLower() == model.Name.ToLower()))
                {
                    return RedirectToAction("Index");
                }
            }
            var result = ObjectMapper.Mapper.Map(model, category);
            result.ModifierUser = User.GetUserId();
            result.Icon = await _saveFileDirectory.DeleteAndSaveFile(model.OldIconPath, model.Icon, _configuration["PathSaveFileDirectory:CategoryIcon"]);
            _categoryRepository.Update(result);
            await _categoryRepository.Save();
            TempData["IsSuccess"] = true;
            return RedirectToAction("Index");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _categoryRepository.GetById(id));
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete(Category model, int id)
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
    }
}
