using DL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class GeneratedFile : BaseEntity
    {
        [Required]
        public FileType FileType { get; set; }

        [Required]
        public string Name { get; set; }

        public string Path { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
