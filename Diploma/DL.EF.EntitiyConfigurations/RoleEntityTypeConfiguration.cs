using DL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using DL.Infrastructure.Roles;

namespace DL.EF.EntitiyConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        readonly List<Role> _roles;

        public RoleEntityTypeConfiguration()
        {
            _roles = new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Name = Roles.Client
                }
            };
        }

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(_roles);
        }
    }
}
