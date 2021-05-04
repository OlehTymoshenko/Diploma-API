using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BL.Models.Auth;
using BL.Interfaces.Subdomains.Auth.Services;
using PL.Utils.Auth;
using Common.Configurations.Sections;

namespace PL.Diploma.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Policies.Client)]
    public class IdentityController : DiplomaApiControllerBase
    {
        readonly IAuthService _authService;

        public IdentityController(IAuthService authService, IOptions<ConnectionStringsSection> options)
        {
            _authService = authService;

            Console.WriteLine($"-----Connection string for postgre {options.Value.NpgsqlConnection}");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResultModel>> Login(LoginModel loginModel)
        {
            var resultModel = await _authService.LoginAsync(loginModel);

            return Ok(resultModel);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            var resultModel = await _authService.RegisterAsync(registerModel);

            return Ok(resultModel);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext.User.Claims);
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("refresh-tokens")]
        public async Task<ActionResult<TokensPairModel>> RefreshTokens(TokensPairModel refreshTokenModel)
        {
            var refreshedTokens = await _authService.RefreshTokens(refreshTokenModel);

            return Ok(refreshedTokens);
        }
    }
}
