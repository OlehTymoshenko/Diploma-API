using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BL.Interfaces.Subdomains.Auth;
using BL.Subdomains.Auth;
using BL.Infrastructure.AutoMapperProfiles;
using System.Linq;

namespace BL.DIExtension
{
    public static class BusinessLayerExtesions
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection serviceCollection)
        {
            // Load automapper assembly to appDomain
            // Without this line action GetAssemblies() method can't load assembly with Profiles for AutoMapper
            AppDomain.CurrentDomain.Load(typeof(AuthProfile).Assembly.GetName());
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add services
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            
            return serviceCollection;
        }
    }
}
