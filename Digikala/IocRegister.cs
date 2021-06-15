using Digikala.Core.Interfaces;
using Digikala.Core.Interfaces.Generic;
using Digikala.Core.Interfaces.Identity;
using Digikala.Core.Services;
using Digikala.Core.Services.Generic;
using Digikala.Core.Services.Identity;
using Digikala.Utility.Convertor;
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

            #region Identity
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            #endregion

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            return services;
        }
    }
}