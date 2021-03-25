using Microsoft.Extensions.Configuration;
using System;

namespace Common.Configurations.Hosting
{
    internal class HerokuUtils
    {
        internal const string NAME_OF_ENVIRONMENT_VARIABLE_FOR_DATABASE_URI = "DATABASE_URL";

        internal string GetPostgreSqlDbConnectionString(IConfiguration configuration)
        {
            var dbConnectionUrl = configuration.GetValue<string>(NAME_OF_ENVIRONMENT_VARIABLE_FOR_DATABASE_URI);

            var dbConnectionUri = new Uri(dbConnectionUrl);

            var dbName = dbConnectionUri.LocalPath.Trim('/');

            var userInfo = dbConnectionUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);


            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={dbConnectionUri.Host};Port={dbConnectionUri.Port};" +
                $"Database={dbName};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }
    }
}
