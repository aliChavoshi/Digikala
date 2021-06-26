using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.AdminPanel.Identity;
using Digikala.Core.Interfaces.Generic;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.FormDto.Public;
using Digikala.DTOs.InputParams.AdminPanel.Home;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Digikala.Core.Services.AdminPanel.Identity
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly IPaginationRepository _pagination;
        public RoleRepository(DigikalaContext context, IPaginationRepository pagination) : base(context)
        {
            _pagination = pagination;
        }

        public async Task<GetAllGenericByPaginationDto<Role>> RolesToList(RoleParamsDto paramsDto)
        {
            IQueryable<Role> result = Context.Role;

            #region Searching

            if (!string.IsNullOrEmpty(paramsDto.FilterTitle))
            {
                result = result.Where(p => p.Title.ToLower().Contains(paramsDto.FilterTitle.ToLower()));
            }

            #endregion

            #region OrderBy

            if (paramsDto.Order == Order.Des)
            {
                switch (paramsDto.OrderFrom)
                {
                    case "0":
                        {
                            result = result.OrderByDescending(x => x.Title);
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
                            result = result.OrderBy(x => x.Title);
                            break;
                        }
                }
            }
            #endregion

            var page = _pagination.CreatePagination(paramsDto.SendTake, result.Count(), paramsDto.SendPageId);

            return new GetAllGenericByPaginationDto<Role>()
            {
                PaginationDto = page,
                List = await result.Skip(page.Skip).Take(paramsDto.SendTake).ToListAsync()
            };
        }

        public async Task<List<SelectListItem>> RolesForSelectList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "لطفا انتخاب کنید",
                }
            };
            var roles = await Context.Role.Select(r => new SelectListItem()
            {
                Text = r.Title,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.AddRange(roles);
            return list;
        }
    }
}