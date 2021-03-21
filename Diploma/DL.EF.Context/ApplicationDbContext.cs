using Microsoft.EntityFrameworkCore;
using System;
using DL.Entities;
using DL.EF.EntitiyConfigurations;

namespace DL.EF.Context
{
    public class ApplicationDbContext : DbContext
    {
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        

        public DbSet<User> Users { get; set; } 

        public DbSet<Role> Roles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        }
    }
}
