using Microsoft.Extensions.DependencyInjection;
using Common.Configurations.Sections;
using Microsoft.Extensions.Configuration;

namespace Common.Configurations
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
