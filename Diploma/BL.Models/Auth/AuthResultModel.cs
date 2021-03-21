using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Auth
{
    public class AuthResultModel
    {
        public UserModel UserModel { get; set; } = new UserModel();

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
