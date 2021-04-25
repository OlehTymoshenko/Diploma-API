using System;
using System.Net;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BL.Interfaces.Subdomains.Auth.Services;
using Common.Configurations.Sections;
using Common.Infrastructure.Exceptions;
using Common.Infrastructure.ErrorMessages;

namespace BL.Subdomains.Auth.Services
{
    public class TokenService : ITokenService
    {
        const string JWT_TOKEN_SIGNATURE_ALGORITHM = SecurityAlgorithms.HmacSha256;
        const int REFRESH_TOKEN_SIZE_IN_BYTES = 64;
        readonly TimeSpan ACCESS_TOKEN_LIFETIME = TimeSpan.FromHours(1);

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
                Expires = DateTime.UtcNow.Add(ACCESS_TOKEN_LIFETIME),
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
                throw new DiplomaApiExpection(AuthErrorMessages.InvalidAccessToken, HttpStatusCode.BadRequest);
            }

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.SignatureAlgorithm.Equals(JWT_TOKEN_SIGNATURE_ALGORITHM, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenDecryptionFailedException(AuthErrorMessages.InvalidAccessToken);
            }

            return claimsPrincipal;
        }
    }
}
