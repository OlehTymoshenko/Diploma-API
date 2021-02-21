using System;
using System.Collections.Generic;
using System.Text;
using DL.Entities.Base;

namespace DL.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreateDateTime { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}