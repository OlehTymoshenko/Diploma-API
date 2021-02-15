using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PL.Utils.Configuration.Sections;

namespace PL.Utils.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddAppConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettingsSection>(configuration.GetSection("AppSettings"));

            services.Configure<ConnectionStringsSection>(configuration.GetSection("ConnectionStrings"));

            return services;
        }
    }
}
