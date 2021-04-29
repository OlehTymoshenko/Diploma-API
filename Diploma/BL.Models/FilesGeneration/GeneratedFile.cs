using System.ComponentModel.DataAnnotations;

namespace BL.Models.FilesPreparation
{
    public class GeneratedFile
    {
        [Required]
        public long Id { get; set; }

        public string FullName { get; set; }

    }
}
