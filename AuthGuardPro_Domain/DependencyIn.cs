
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StayEasePro_Application.CommonRepos.Contracts;
using StayEasePro_Application.CommonRepos.Services;
using StayEasePro_Core.DbRepos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Core
{
    public static class DependencyIn
    {
        public static IServiceCollection CoreDI(this IServiceCollection services)
        {
            //services.AddDbContext<UsersContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
            return services;
        }
    }
}
