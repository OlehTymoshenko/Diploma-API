using Microsoft.EntityFrameworkCore;
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

        public DbSet<Degree> Degrees { get; set; }

        public DbSet<FileType> FileTypes { get; set; }

        public DbSet<GeneratedFile> GeneratedFiles { get; set; }

        public DbSet<PublishingHouse> PublishingHouses { get; set; }

        public DbSet<Scientist> Scientists { get; set; }

        public DbSet<UniversityDepartment> UniversityDepartments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileTypeEntityTypeConfiguration());
        }
    }
}
