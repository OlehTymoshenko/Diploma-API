using Microsoft.EntityFrameworkCore;
using System;
using DL.Entities;
using MigrationProject = DL.EF.Migrations.Migrations;

namespace DL.EF.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(npgsqlBuilder =>
                {
                    npgsqlBuilder.MigrationsAssembly(typeof(MigrationProject).Assembly.GetName().Name);
                });
            }
        }


        public DbSet<User> Users { get; set; } 

        public DbSet<Role> Roles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
