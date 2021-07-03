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

        public async Task<IActionResult> Create()
        {
            await CategoriesForSelectList();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
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
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(CategoryParamsDto paramsDto)
        {
            ViewBag.FilterRoot = paramsDto.FilterRoot;
            ViewBag.FilterTitle = paramsDto.FilterTitle;
            return View(await _categoryRepository.CategoriesToList(paramsDto));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetById(id);
            var model = ObjectMapper.Mapper.Map<Category, EditCategoryViewModel>(category);
            await CategoriesForSelectList(category.ParentId ?? 0);
            return PartialView(model);
        }
    }
}
