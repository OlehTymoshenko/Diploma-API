using DL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.EF.EntitiyConfigurations
{
    public class PublishingHouseEntityTypeConfiguration : IEntityTypeConfiguration<PublishingHouse>
    {
        readonly List<PublishingHouse> _publishingHouses;

        public PublishingHouseEntityTypeConfiguration()
        {
            _publishingHouses = new()
            {
                new() {  Id = 1, Name = "видавництво Національного аерокосмічного університету ім. М. Є. Жуковського \"ХАІ\"" }
            };
        }

        public void Configure(EntityTypeBuilder<PublishingHouse> builder)
        {
            builder.HasData(_publishingHouses);
        }
    }
}
