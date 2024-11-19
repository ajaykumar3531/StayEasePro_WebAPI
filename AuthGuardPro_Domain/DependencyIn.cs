
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            return services;
        }
    }
}
