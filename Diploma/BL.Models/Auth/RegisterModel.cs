using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Models.Auth
{
    public class RegisterModel
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
