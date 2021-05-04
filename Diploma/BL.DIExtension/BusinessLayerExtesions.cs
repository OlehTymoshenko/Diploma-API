using System;
using Microsoft.Extensions.DependencyInjection;
using BL.Interfaces.Subdomains.Auth.Services;
using BL.Interfaces.Subdomains.DataForFiles.Services;
using BL.Interfaces.Subdomains.FilesGeneration;
using BL.Subdomains.Auth.Services;
using BL.Subdomains.DataForFiles.Services;
using BL.Subdomains.FilesGeneration;
using BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml;
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

            #region Auth subdomain
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            #endregion

            #region Data for file subdomain
            serviceCollection.AddScoped<IPublishingHouseService, PublishingHouseService>();
            serviceCollection.AddScoped<IUniversityDepartmentService, UniversityDepartmentService>();
            serviceCollection.AddScoped<IDegreeService, DegreeService>();
            serviceCollection.AddScoped<IScientistService, ScientistService>();
            #endregion

            #region Files generation subdomain
            serviceCollection.AddScoped<IFilesGenerationService, FilesGenerationService>();

            serviceCollection.AddScoped<IFileHandlerFactory, FileHandlerFactory>();
            serviceCollection.AddScoped<INotesOfAuthorsHandler, NotesOfAuthorsInDocxHandler>();
            #endregion

            return serviceCollection;
        }
    }
}
