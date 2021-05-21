using System;
using DL.Entities.Enums;

namespace BL.Models.FilesGeneration
{
    public class DescriptionOfGeneratedFile
    {
        public string Name { get; set; }

        public FileType Type { get; set; }

        public FileFormat Format { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
