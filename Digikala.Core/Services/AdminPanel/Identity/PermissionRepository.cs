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
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly IPaginationRepository _pagination;
        public PermissionRepository(DigikalaContext context, IPaginationRepository pagination) : base(context)
        {
            _pagination = pagination;
        }

        public async Task<List<SelectListItem>> PermissionsForSelectList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "لطفا انتخاب کنید",
                }
            };

            var permissions = await Context.Permission.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.AddRange(permissions);
            return list;
        }

        public async Task<GetAllGenericByPaginationDto<Permission>> PermissionsToList(PermissionParamsDto paramsDto)
        {
            IQueryable<Permission> result = Context.Permission;

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

            return new GetAllGenericByPaginationDto<Permission>()
            {
                PaginationDto = page,
                List = await result.Skip(page.Skip).Take(paramsDto.SendTake).ToListAsync()
            };
        }

        public async Task DeleteSave(Permission permission)
        {
            permission.IsDeleted = true;
            Update(permission);
            await Save();
        }
    }
}