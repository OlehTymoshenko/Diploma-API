using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Utils.Auth
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {



            return services;
        }

        public static IServiceCollection AddAuthorization(this IServiceCollection services)
        {



            return services;
        }

    }
}
