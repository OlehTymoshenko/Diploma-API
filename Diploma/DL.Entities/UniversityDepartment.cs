using DL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public  class UniversityDepartment : BaseEntity
    {
        [Required]
        public string FullName { get; set; }

        public string ShortName { get; set; }
    }
}
