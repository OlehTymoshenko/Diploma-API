using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class SaveDegreeModel
    {
        [Required]
        public string Name { get; set; }
    }
}
