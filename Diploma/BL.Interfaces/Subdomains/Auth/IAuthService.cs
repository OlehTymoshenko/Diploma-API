using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BL.Models.Auth;

namespace BL.Interfaces.Subdomains.Auth
{
    public interface IAuthService
    {
        Task<AuthResultModel> LoginAsync(LoginModel loginModel);

        Task LogoutAsync(IEnumerable<Claim> claims);

        Task<AuthResultModel> RegisterAsync(RegisterModel registerModel);

        Task<TokensPairModel> RefreshTokens(TokensPairModel refreshTokensModel);
    }
}
