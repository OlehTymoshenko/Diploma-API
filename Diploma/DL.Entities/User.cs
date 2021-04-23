using System;
using System.Collections.Generic;
using DL.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace DL.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime CreateDateTime { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}