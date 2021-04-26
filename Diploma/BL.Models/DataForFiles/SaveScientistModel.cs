using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class SaveScientistModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public ICollection<int> DegreesIds { get; set; } = new List<int>();
    }
}
