﻿using Digikala.Core.Classes;
using Digikala.Core.Interfaces;
using Digikala.Core.Services;
using Digikala.DataAccessLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Digikala.Core.Interfaces.Generic;
using Digikala.Core.Services.Generic;
using Digikala.Utility.Convertor;

namespace Digikala
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);//1Month
            });
            #endregion

            #region Context

            services.AddDbContextPool<DigikalaContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DigikalaConnection"));
            });

            #endregion

            #region IOC
            services.AddMyServiceCollection();
            #endregion

            #region AutoMapping
            services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Caching
            services.AddResponseCaching();
            services.AddMemoryCache();
            #endregion

            #region Routing

            services.AddMvc(options => options.EnableEndpointRouting = false);

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseResponseCaching();
        }
    }
}
