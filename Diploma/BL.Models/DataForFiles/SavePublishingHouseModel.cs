using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class SavePublishingHouseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
