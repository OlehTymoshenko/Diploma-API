using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DL.EF.Context;


namespace DL.EF.Migrations
{
    class ApplicationDesignTimeDbContext : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public const string CONFIG_FILE_NAME = "appsettings.json";
        public const string CONNECTION_STRING_NAME = "NpgsqlConnection";

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile(CONFIG_FILE_NAME, optional: false, reloadOnChange: true);
            var config = configBuilder.Build();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            dbContextOptionsBuilder.UseNpgsql(config.GetConnectionString(CONNECTION_STRING_NAME),
                npgsqlOpt => npgsqlOpt.MigrationsAssembly(typeof(DummyProgram).Assembly.GetName().Name)); ;

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
