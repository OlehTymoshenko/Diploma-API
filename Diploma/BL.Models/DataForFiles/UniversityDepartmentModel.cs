﻿using System.ComponentModel.DataAnnotations;

namespace BL.Models.DataForFiles
{
    public class UniversityDepartmentModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string ShortName { get; set; }
    }
}
