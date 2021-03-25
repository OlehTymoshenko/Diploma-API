using Microsoft.Extensions.Configuration;

namespace Common.Configurations.Hosting
{
    public class HostingService
    {
        private const string NAME_OF_ENV_VAR_FOR_HOSTING_NAME = "HOSTING";

        public string GetPostgreSqlDbConnectionString(IConfiguration configuration)
        {
            string resultConnectionString = string.Empty;

            var hostingService = configuration.GetValue<Hosting>(NAME_OF_ENV_VAR_FOR_HOSTING_NAME);

            switch (hostingService)
            {
                case Hosting.Localhost:
                    {
                        resultConnectionString = configuration.GetConnectionString("NpgsqlConnection");
                    }
                    break;
                case Hosting.Heroku:
                    {
                        var herokuUtils = new HerokuUtils();
                        resultConnectionString = herokuUtils.GetPostgreSqlDbConnectionString(configuration);
                    }
                    break;
            }

            return resultConnectionString;
        }
    }
}
