using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using BookShop.Api.Contexts;
using BookShop.Api.Models;
using BookShop.Api.RequestResponse.Request;
using BookShop.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace BookShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding MVC, Version and FluentValidation
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation();

            // Register the Swagger for creating better documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookShop API", Version = "v1" });
            });

            // Register the DbContext with OpenIddict on the container
            services.AddDbContext<ApplicationDbContext>(
                o =>
                {
                    // Configure the context to use Microsoft SQL Server.
                    o.UseSqlServer(Configuration.GetConnectionString("BookShopDbConnectionString"));

                    // Register the entity sets needed by OpenIddict.
                    o.UseOpenIddict();
                });

            // Register the Identity services.
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Register the Identity Service Options.
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = false;
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });


            // Register the OpenIddict services.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and entities.
                    options.UseEntityFrameworkCore()
                        .UseDbContext<ApplicationDbContext>();
                })

                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();

                    // Enable the token endpoint (required to use the password flow).
                    options.EnableAuthorizationEndpoint("/api/connect/authorize")
                        .EnableLogoutEndpoint("/api/connect/logout")
                        .EnableTokenEndpoint("/api/connect/token");

                    // Allow client applications to use the grant_type=password flow also the Allow RefreshToken.
                    options.AllowAuthorizationCodeFlow()
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();

                    // Accept token requests that don't specify a client_id.
                    options.AcceptAnonymousClients();
                })

                .AddValidation();

            // Register the Authentication Scheme
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OAuthValidationDefaults.AuthenticationScheme;
            });

            // Register the Repository service and RequestModel
            services.AddTransient<IValidator<AddBookRequestModel>, AddBookRequestModelValidator>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IValidator<AddCategoryRequestModel>, AddCategoryRequestModelValidation>();
            services.AddTransient<IValidator<UpdateCategoryRequestModel>, UpdateCategoryRequestModelValidation>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IValidator<AuthorRequestModel>, AuthorRequestModelValidator>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IValidator<RegistrationRequestModel>, RegistrationRequestModelValidator>();
            services.AddTransient<ILoginService, LoginService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShop API V1");
            });


            // Register the `User Authentication` services in Configure Container
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
