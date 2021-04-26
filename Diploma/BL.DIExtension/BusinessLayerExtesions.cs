using Microsoft.Extensions.DependencyInjection;
using System;
using BL.Interfaces.Subdomains.Auth.Services;
using BL.Interfaces.Subdomains.DataForFiles.Services;
using BL.Subdomains.Auth.Services;
using BL.Subdomains.DataForFiles.Services;
using BL.Infrastructure.AutoMapperProfiles;

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
            serviceCollection.AddScoped<IPublishingHouseService, PublishingHouseService>();
            serviceCollection.AddScoped<IUniversityDepartmentService, UniversityDepartmentService>();
            serviceCollection.AddScoped<IDegreeService, DegreeService>();
            serviceCollection.AddScoped<IScientistService, ScientistService>();


            return serviceCollection;
        }
    }
}
