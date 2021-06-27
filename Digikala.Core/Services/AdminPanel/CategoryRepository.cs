using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.AdminPanel;
using Digikala.Core.Interfaces.Generic;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Store;
using Digikala.DTOs.FormDto.Public;
using Digikala.DTOs.InputParams.AdminPanel.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Digikala.Core.Services.AdminPanel
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly IPaginationRepository _pagination;
        public CategoryRepository(DigikalaContext context, IPaginationRepository pagination) : base(context)
        {
            _pagination = pagination;
        }

        public async Task<List<SelectListItem>> CategoriesForSelectList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "لطفا انتخاب کنید",
                }
            };

            var categories = await Context.Category.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.AddRange(categories);
            return list;
        }

        public async Task<GetAllGenericByPaginationDto<Category>> CategoriesToList(CategoryParamsDto paramsDto)
        {
            IQueryable<Category> result = Context.Category.Include(x=>x.UserCreator);

            #region Searching
            //TODO Search both
            if (!string.IsNullOrEmpty(paramsDto.FilterRoot))
            {
                var ids = result
                    .Where(x => x.ParentId == null && x.Name.ToLower().Contains(paramsDto.FilterRoot.ToLower()))
                    .Select(x => x.Id);
                result = result.Where(p => ids.Any(x => p.Id == x || p.ParentId == x));
            }
            if (!string.IsNullOrEmpty(paramsDto.FilterTitle))
            {
                result = result.Where(p => p.Name.ToLower().Contains(paramsDto.FilterTitle.ToLower()));
            }
            #endregion

            #region OrderBy

            if (paramsDto.Order == Order.Des)
            {
                switch (paramsDto.OrderFrom)
                {
                    case "0":
                    {
                        result = result.OrderByDescending(x => x.Name);
                        break;
                    }
                }
            }
            else
            {
                switch (paramsDto.OrderFrom)
                {
                    case "0":
                    {
                        result = result.OrderBy(x => x.Name);
                        break;
                    }
                }
            }
            #endregion

            var page = _pagination.CreatePagination(paramsDto.SendTake, result.Count(), paramsDto.SendPageId);

            return new GetAllGenericByPaginationDto<Category>()
            {
                PaginationDto = page,
                List = await result.Skip(page.Skip).Take(paramsDto.SendTake).ToListAsync()
            };
        }
    }
}