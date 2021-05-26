using Microsoft.Extensions.DependencyInjection;
using Common.Configurations.Sections;
using Microsoft.Extensions.Configuration;
using Common.Configurations.Hosting;

namespace Common.Configurations
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddAppConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettingsSection>(configuration.GetSection("AppSettings"));
            services.Configure<ConnectionStringsSection>(configuration.GetSection("ConnectionStrings"));
            services.Configure<MorpherSection>(configuration.GetSection("Morpher"));


            PostConfigureConnectionsStringsSection(services, configuration);


            return services;
        }

        private static void PostConfigureConnectionsStringsSection(IServiceCollection services, IConfiguration configuration)
        {
            var hostingService = new HostingService();

            services.PostConfigureAll<ConnectionStringsSection>(opt => 
                opt.NpgsqlConnection = hostingService.GetPostgreSqlDbConnectionString(configuration));
        }
    }
}
