using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BL.Models.Auth
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password min lenght is 6")]
        [MaxLength(50, ErrorMessage = "Password max lenght is 50")]
        public string Password { get; set; }
    }
}
