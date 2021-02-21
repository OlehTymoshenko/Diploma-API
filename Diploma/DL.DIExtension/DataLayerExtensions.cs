using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Common.Configurations.Sections;
using Microsoft.Extensions.Options;
using DL.EF.Migrations;
using DL.EF.Context;
using DL.Interfaces.UnitOfWork;
using DL.Interfaces.Repositories;
using DL.Repositories;
using DL.UnitOfWork;

namespace DL.DIExtesion
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IOptions<ConnectionStringsSection> options)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(options.Value.NpgsqlConnection, 
                    npgsqlOpt => npgsqlOpt.MigrationsAssembly(typeof(Migrations).Assembly.GetName().Name));
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
