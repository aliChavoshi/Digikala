using System.Collections.Generic;
using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services.Generic;
using Digikala.DataAccessLayer.Context;
using Digikala.DataAccessLayer.Entities.Identity;
using Digikala.DTOs.InputParams.AdminPanel.Home;
using Digikala.Utility.Convertor;
using System.Linq;
using System.Threading.Tasks;
using Digikala.Core.Interfaces.Generic;
using Digikala.DTOs.FormDto.Public;
using Microsoft.EntityFrameworkCore;

namespace Digikala.Core.Services.Identity
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly DigikalaContext _context;
        private readonly IPaginationRepository _pagination;

        public RolePermissionRepository(DigikalaContext context, IPaginationRepository pagination) : base(context)
        {
            _context = context;
            _pagination = pagination;
        }

        public async Task<bool> IsRoleHavePermission(int permissionId, int roleId)
        {
            return await IsExist(x => x.RoleId == roleId && x.PermissionId == permissionId);
        }

        public async Task AddPermissionsIdToRole(CreateRolePermissionDto model)
        {
            var perIdForRoleIdDb = await Context.RolePermission
                .Where(x => x.RoleId == model.RoleId).Select(x => x.PermissionId).ToListAsync();

            #region Distinct

            //distinct permission id & expire date 
            var listPermission = new List<int>();
            var listExpDate = new List<string>();
            for (int i = 0; i < model.PermissionId.Count; i++)
            {
                if (perIdForRoleIdDb.Any(x => x == model.PermissionId[i]))
                {
                    continue;
                }
                listPermission.Add(model.PermissionId[i]);
                listExpDate.Add(model.ExpireRolePermission[i]);
            }

            #endregion

            if (listPermission.Any())
            {
                await _context.RolePermission
                    .AddRangeAsync(listPermission.Select((permissionId, index) =>
                        new RolePermission
                        {
                            PermissionId = permissionId,
                            ExpireRolePermission = listExpDate[index] == "0" ? null : listExpDate[index].ToMiladi(),
                            RoleId = model.RoleId
                        }));
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GetAllGenericByPaginationDto<RolePermission>> RolePermissionToList(RolePermissionParamsDto paramsDto)
        {
            IQueryable<RolePermission> result = Context.RolePermission
                .Include(x => x.Permission)
                .Include(x => x.Role);

            #region FilterDate

            if (!string.IsNullOrEmpty(paramsDto.StartDate))
            {
                result = result.Where(s => s.ExpireRolePermission.Value.Date >= paramsDto.StartDate.ToMiladi().Date);
            }
            if (!string.IsNullOrEmpty(paramsDto.EndDate))
            {
                result = result.Where(s => s.ExpireRolePermission.Value.Date <= paramsDto.EndDate.ToMiladi().Date);
            }

            #endregion

            #region Searching
            if (!string.IsNullOrEmpty(paramsDto.FilterRole))
            {
                result = result.Where(p => p.Role.Title.ToLower().Contains(paramsDto.FilterRole.ToLower()));
            }
            if (!string.IsNullOrEmpty(paramsDto.FilterPermission))
            {
                result = result.Where(p => p.Permission.Name.ToLower().Contains(paramsDto.FilterPermission.ToLower()));
            }
            #endregion

            #region OrderBy

            if (paramsDto.Order == Order.Des)
            {
                switch (paramsDto.OrderFrom)
                {
                    case "0":
                        {
                            result = result.OrderByDescending(x => x.Role.Title);
                            break;
                        }
                    case "1":
                        {
                            result = result.OrderByDescending(x => x.Permission.Name);
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
                            result = result.OrderBy(x => x.Role.Title);
                            break;
                        }
                    case "1":
                        {
                            result = result.OrderBy(x => x.Permission.Name);
                            break;
                        }
                }
            }
            #endregion

            var page = _pagination.CreatePagination(paramsDto.SendTake, result.Count(), paramsDto.SendPageId);

            return new GetAllGenericByPaginationDto<RolePermission>()
            {
                PaginationDto = page,
                List = await result.Skip(page.Skip).Take(paramsDto.SendTake).ToListAsync()
            };
        }

        public async Task UpdateSaveRolePermission(RolePermission model, string expireDate = "")
        {
            if (string.IsNullOrEmpty(expireDate))
            {
                model.ExpireRolePermission = null;
            }
            else
            {
                model.ExpireRolePermission = expireDate.ToMiladi();
            }
            Update(model);
            await Save();
        }
    }
}