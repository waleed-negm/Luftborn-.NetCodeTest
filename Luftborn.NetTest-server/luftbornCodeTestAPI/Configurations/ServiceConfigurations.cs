using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Application.IunitOfWork;
using Core.Application.Services;
using Core.Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Services;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace luftbornCodeTestAPI.Configurations
{
    public static class ServiceConfigurations
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            
            services.AddHttpContextAccessor();
            services.AddIdentity<User, IdentityRole>(o => {
                o.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 8,
                    RequireLowercase = true,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = false
                };
                o.User.RequireUniqueEmail = true;
                o.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<LuftbornContext>().AddDefaultTokenProviders();
        }
    }
}
