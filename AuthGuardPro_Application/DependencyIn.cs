using AuthGuardPro_Application.Repos.Services;
using Microsoft.Extensions.DependencyInjection;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.CommonRepos.Services;
using StayEasePro_Application.Repos.Contracts;
using StayEasePro_Application.Repos.Services;
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
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<ICommonService, CommonService>();
            return services;
        }
    }
}
