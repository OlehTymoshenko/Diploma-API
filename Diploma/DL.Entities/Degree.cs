using DL.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class Degree : BaseEntity
    {
        [Required]
        public string Name { get; set; } 

        public ICollection<Scientist> Scientists { get; set; } = new List<Scientist>();
    }
}
