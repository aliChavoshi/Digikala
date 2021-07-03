using Digikala.Core.Interfaces;
using Digikala.Core.Interfaces.AdminPanel;
using Digikala.Core.Interfaces.AdminPanel.Identity;
using Digikala.Core.Interfaces.Generic;
using Digikala.Core.Services;
using Digikala.Core.Services.AdminPanel;
using Digikala.Core.Services.AdminPanel.Identity;
using Digikala.Core.Services.Generic;
using Digikala.Utility.Convertor;
using Digikala.Utility.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Digikala
{
    public static class IocRegister
    {
        public static IServiceCollection AddMyServiceCollection(this IServiceCollection services)
        {
            #region Generic
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPaginationRepository,PaginationRepository>();
            #endregion

            #region AdminPanel

            #region Identity
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            #endregion

            services.AddScoped<ICategoryRepository,CategoryRepository>();

            #endregion


            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<ISaveFileDirectory,SaveFileDirectory>();
            return services;
        }
    }
}