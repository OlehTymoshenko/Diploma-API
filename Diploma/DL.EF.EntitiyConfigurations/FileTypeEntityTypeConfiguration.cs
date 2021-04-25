using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DL.Entities.Enums;

namespace DL.EF.EntitiyConfigurations
{
    public class FileTypeEntityTypeConfiguration : IEntityTypeConfiguration<FileType>
    {
        readonly List<FileType> _fileTypes;

        public FileTypeEntityTypeConfiguration()
        {
            _fileTypes = new()
            {
                new() { Id = 1, Type = AvailableFileTypes.NoteOfAuthors },
                new() { Id = 2, Type = AvailableFileTypes.ExpertCommissionAct },
                new() { Id = 3, Type = AvailableFileTypes.ProtocolOfMeetingOfExpertCommission }
            };
        }

        public void Configure(EntityTypeBuilder<FileType> builder)
        {
            builder.HasData(_fileTypes);
        }
    }
}
