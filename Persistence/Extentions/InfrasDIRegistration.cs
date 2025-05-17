using System.Text;
using Core.Interface;
using Core.Services;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Repositories;

namespace Persistence.Extentions
{
    public static class InfrasDIRegistration
    {
        public static IServiceCollection AddInfrasDIRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("myDbConnectionString")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICompanyRespository, CompanyRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IJwtServiceProvider, JwtServiceProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            ///// identitycore setup
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            })
             .AddRoles<IdentityRole>() // ✅ This must come BEFORE .AddEntityFrameworkStores
             .AddEntityFrameworkStores<AppDbContext>() // ✅ Registers IUserStore and IRoleStore
             .AddDefaultTokenProviders()
             .AddRoleManager<RoleManager<IdentityRole>>() // Optional if needed explicitly
             .AddSignInManager<SignInManager<ApplicationUser>>();


            /// jwt bearer setup 

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>

            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtConfig:Issuer"],
                    ValidAudience = configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsJsonAsync(new ProblemDetails
                            {
                                Status = 401,
                                Title = "Unauthorized",
                                Detail = "Authentication required"
                            });
                        }
                    },

                    OnForbidden = async context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsJsonAsync(new ProblemDetails
                            {
                                Status = 403,
                                Title = "Forbidden",
                                Detail = "Insufficient permissions"
                            });
                        }
                    },

                    OnAuthenticationFailed = async context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var error = context.Exception switch
                            {
                                SecurityTokenExpiredException => "Token expired",
                                _ => "Invalid authentication token"
                            };

                            await context.Response.WriteAsJsonAsync(new ProblemDetails
                            {
                                Status = 401,
                                Title = "Authentication Failed",
                                Detail = error
                            });
                        }
                    }
                };
            });

            return services;
        }


    }
}