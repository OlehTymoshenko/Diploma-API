using System;
using System.Collections.Generic;
using System.Security.Claims;
using BL.Interfaces.Subdomains.Auth;
using Common.Configurations.Sections;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Common.Infrastructure.Exceptions;
using System.Net;

namespace BL.Subdomains.Auth
{
    public class TokenService : ITokenService
    {
        const string JWT_TOKEN_SIGNATURE_ALGORITHM = SecurityAlgorithms.HmacSha256;
        const int REFRESH_TOKEN_SIZE_IN_BYTES = 64;
        readonly AppSettingsSection _appSettings;

        public TokenService(IOptions<AppSettingsSection> appSettingsOption)
        {
            _appSettings = appSettingsOption.Value;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var signinCredentials = new SigningCredentials(securityKey, JWT_TOKEN_SIGNATURE_ALGORITHM);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(2), //AddHours(1),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = signinCredentials
            };

            var jwtAccessToken = new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescription);

            return jwtAccessToken;
        }

        public string GenerateRefreshToken()
        {
            using var rng = new RNGCryptoServiceProvider();

            var refreshToken = new byte[REFRESH_TOKEN_SIZE_IN_BYTES];

            rng.GetBytes(refreshToken);

            return Convert.ToBase64String(refreshToken);
        }

        public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false, // false because we want get claims from token, and it's already known, that token is expired
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret))
            };

            SecurityToken securityToken;
            ClaimsPrincipal claimsPrincipal;

            try
            {
                claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch(Exception)
            {
                throw new DiplomaApiExpection(ErrorMessages.InvalidAccessToken, HttpStatusCode.BadRequest);
            }

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.SignatureAlgorithm.Equals(JWT_TOKEN_SIGNATURE_ALGORITHM, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenDecryptionFailedException(ErrorMessages.InvalidAccessToken);
            }

            return claimsPrincipal;
        }
    }
}
