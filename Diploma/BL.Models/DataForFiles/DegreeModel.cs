using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class DegreeModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
