using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class SaveUniversityDepartmentModel
    {
        [Required]
        public string FullName { get; set; }

        public string ShortName { get; set; }
    }
}
