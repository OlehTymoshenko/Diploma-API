using DL.Entities.Base;
using DL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class FileType : BaseEntity
    {
        [Required]
        public AvailableFileTypes Type { get; set; }
    }
}
