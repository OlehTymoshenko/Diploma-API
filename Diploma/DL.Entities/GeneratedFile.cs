using DL.Entities.Base;
using DL.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class GeneratedFile : BaseEntity
    {
        [Required]
        public FileType Type { get; set; }

        [Required]
        public FileFormat Format { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
