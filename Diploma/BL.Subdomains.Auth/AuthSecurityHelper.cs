using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL.Subdomains.Auth
{
    public class AuthSecurityHelper
    {
        private readonly byte[] _passwordHashSalt;

        public AuthSecurityHelper(string passwordHashSalt)
        {
            _passwordHashSalt = Encoding.UTF8.GetBytes(passwordHashSalt);
        }

        public string GetPasswordHash(string password)
        {
            var hash = new HMACSHA256(_passwordHashSalt).ComputeHash(Encoding.UTF8.GetBytes(password));

            return Encoding.UTF8.GetString(hash);
        }
    }
}
