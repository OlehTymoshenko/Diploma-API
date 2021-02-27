using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Common.Configurations.Sections;
using Microsoft.Extensions.Options;
using DL.EF.Context;
using DL.Interfaces.UnitOfWork;
using DL.Interfaces.Repositories;
using DL.Repositories;
using Microsoft.Extensions.Configuration;

namespace DL.DIExtesion
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("NpgsqlConnection"), 
                    npgsqlOpt => npgsqlOpt.MigrationsAssembly("DL.EF.Migrations"));
            });

            services.PerformMigration();

            services.AddScoped<IUnitOfWork, DL.UnitOfWork.UnitOfWork>();

            services.AddScoped<IUsersRepository, UsersReporitory>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

            return services;
        }

        private static void PerformMigration(this IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
