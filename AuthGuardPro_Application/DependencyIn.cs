using AuthGuardPro_Application.Repos.Contracts;
using AuthGuardPro_Application.Repos.Services;
using AuthGuardPro_Infrastucture.Repository.Contracts;
using AuthGuardPro_Infrastucture.Repository.Services;
using Microsoft.Extensions.DependencyInjection;
using StayEasePro_Core.Entities;

namespace AuthGuardPro_Application
{
    public static class DependencyIn
    {
        public static IServiceCollection ApplicationDI(this IServiceCollection services)
        {
           
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<ILoggerService, LoggerService>();

            return services;
        }
    }
}
