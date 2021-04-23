using System.Collections.Generic;
using DL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class Scientist : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        ICollection<Degree> Degrees { get; set; } = new List<Degree>();
    }
}
