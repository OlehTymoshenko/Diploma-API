using DL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class PublishingHouse : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
