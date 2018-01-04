using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Login.Data;
using Login.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Login.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LoginDbContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("LoginConnectionString"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
        .AddEntityFrameworkStores<LoginDbContext>()
        .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            //services.ConfigureApplicationCookie(config =>
            //{
            //    config.Events =

            //      new CookieAuthenticationEvents()
            //      {
            //          OnRedirectToLogin = (ctx) =>
            //          {
            //              if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            //              {
            //                  ctx.Response.StatusCode = 401;
            //              }

            //              return Task.CompletedTask;
            //          },
            //          OnRedirectToAccessDenied = (ctx) =>
            //          {
            //              if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
            //              {
            //                  ctx.Response.StatusCode = 403;
            //              }

            //              return Task.CompletedTask;
            //          }
            //      };
            //});



            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //});


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAnyOrigin"));
            });


            services.AddScoped<ILoginRepository, LoginRepository>();

            //services.AddAuthorization(options =>
            //{
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            //});

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _config["TokenIssuer"],
                        ValidAudience = _config["TokenAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityKey"]))
                    };
                });


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAll");

            //app.UseJwtBearerAuthentication(new JwtBearerOptions()
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidIssuer = "http://mycodecamp.org",
            //        ValidAudience = "http://mycodecamp.org",
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VERYLONGKEYVALUETHATISSECURE")),
            //        ValidateLifetime = true
            //    }
            //});

            //app.UseAuthentication();

            app.UseIdentity();

            //app.UseMvc();
            app.UseMvcWithDefaultRoute();
        }
    }
}
