﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        // dotnet core allow us to inject some
        // basic framework services 
        public Startup(IConfiguration config) =>
            _config = config;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddIdentity<StoredUser, IdentityRole>(cfg =>
                {
                    cfg.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DutchContext>();

            services
                .AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };
                });

            // Added DutchContext to the container to be resolved
            // when other services require it
            //
            // Also, configured DbContext to use a provider
            services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlite(_config.GetConnectionString("DutchConnectionString"));
            });

            services.AddAutoMapper();

            services.AddScoped<IDutchRepository, DutchRepository>();

            services.AddTransient<DutchSeeder>();

            // Add required services by MVC
            services
                .AddMvc()
                // Support latest features of MVC
                .SetCompatibilityVersion(CompatibilityVersion.Latest)

                .AddJsonOptions(cfg =>
                    cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Registering services
            services.AddTransient<IMailService, NullMailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) // equivalent to -> env.IsEnvironment("Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseAuthentication();

            // Allows to serve static files from default path 'wwwroot'
            app.UseStaticFiles();

            // Extension from OdeToCode.UseNodeModules
            app.UseNodeModules(env);

            // Activate MVC
            app.UseMvc(cfg =>
            {
                cfg.MapRoute(
                    "Default",
                    "/{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
        }
    }
}
