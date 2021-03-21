using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Utils.Auth
{
    public class Policies
    {
        public const string Client = nameof(Client);

        internal static AuthorizationPolicy ClientAuthPolicy
        {
            get
            {
                return new AuthorizationPolicyBuilder().RequireRole("client").Build();
            }
        }

    }
}
