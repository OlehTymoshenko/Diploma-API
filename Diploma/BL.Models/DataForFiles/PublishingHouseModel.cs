using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class PublishingHouseModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
