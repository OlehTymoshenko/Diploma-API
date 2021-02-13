using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Utils.Configuration.Sections;

namespace Utils.Configuration
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
