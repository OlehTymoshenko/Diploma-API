using System.Collections.Generic;
using System.Security.Claims;

namespace BL.Interfaces.Subdomains.Auth.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token);
    }
}
