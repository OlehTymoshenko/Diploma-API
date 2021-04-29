using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using AutoMapper;
using DL.Entities;
using DL.Interfaces.UnitOfWork;
using DL.Infrastructure.Roles;
using BL.Models.Auth;
using BL.Interfaces.Subdomains.Auth.Services;
using Common.Infrastructure.Exceptions;
using Common.Infrastructure.ErrorMessages;
using Common.Configurations.Sections;

namespace BL.Subdomains.Auth.Services
{
    public class AuthService : IAuthService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly ITokenService _tokenService;
        readonly AppSettingsSection _appSettings;

        readonly TimeSpan REFRESH_TOKEN_LIFETIME = TimeSpan.FromDays(90);

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITokenService tokenService,
            IOptions<AppSettingsSection> appSettingOption)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettingOption.Value;
            _tokenService = tokenService;
        }

        public async Task<AuthResultModel> LoginAsync(LoginModel loginModel)
        {
            var securityHelper = new AuthSecurityHelper(_appSettings.PasswordSalt);

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(
                u => (u.Email == loginModel.Email) && (u.Password == securityHelper.GetPasswordHash(loginModel.Password)), 
                u => u.Roles);

            if (user == null)
            {
                throw new DiplomaApiExpection(AuthErrorMessages.InvalidCredentials, HttpStatusCode.Unauthorized);
            }

            var tokensPair = await GenerateTokensPairAsync(user);

            return new AuthResultModel()
            {
                UserModel = _mapper.Map<UserModel>(user),
                AccessToken = tokensPair.AccessToken,
                RefreshToken = tokensPair.RefreshToken
            };
        }

        public async Task<AuthResultModel> RegisterAsync(RegisterModel registerModel)
        {
            // check if the user already exists in a DB
            var isEmailNew = (await _unitOfWork.Users.CountAsync(u => u.Email == registerModel.Email)) == 0;

            if(!isEmailNew)
            {
                throw new DiplomaApiExpection(AuthErrorMessages.EmailAlreadyExistsInDB, HttpStatusCode.BadRequest);
            }

            // Creation new User entity
            var user = _mapper.Map<User>(registerModel);
            
            user.Password = new AuthSecurityHelper(_appSettings.PasswordSalt).GetPasswordHash(user.Password);

            var role = await _unitOfWork.Roles.SingleOrDefaultAsync(r => r.Name == Roles.Client);
            user.Roles.Add(role);
            
            await _unitOfWork.Users.AddAsync(user);
            _unitOfWork.SaveChanges();

            var tokensPair = await GenerateTokensPairAsync(user);

            return new AuthResultModel() 
            {
                UserModel = _mapper.Map<UserModel>(user),
                AccessToken = tokensPair.AccessToken,
                RefreshToken = tokensPair.RefreshToken
            };
        }

        public async Task<TokensPairModel> RefreshTokens(TokensPairModel refreshTokensModel)
        {
            var claimsPrincipal = _tokenService.GetClaimsPrincipalFromExpiredToken(refreshTokensModel.AccessToken);

            var userEmail = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == userEmail, u => u.Roles);

            // delete old refresh token
            var oldRefreshToken = await _unitOfWork.RefreshTokens.SingleOrDefaultAsync(t => t.Token == refreshTokensModel.RefreshToken);
            if (oldRefreshToken == null)
            {
                throw new DiplomaApiExpection(AuthErrorMessages.InvalidRefreshToken, HttpStatusCode.BadRequest);
            }
            
            _unitOfWork.RefreshTokens.Delete(oldRefreshToken);
            _unitOfWork.SaveChanges();

            return await GenerateTokensPairAsync(user);
        }

        public async Task LogoutAsync(IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email);

            if(user == null)
            {
                throw new DiplomaApiExpection(AuthErrorMessages.UserWithProvidedEmailDoesntExists, HttpStatusCode.Gone);
            }

            var tokens = await _unitOfWork.RefreshTokens.SelectAsync(t => t.UserId == user.Id);

            if(tokens == null)
            {
                 throw new DiplomaApiExpection(AuthErrorMessages.RefreshTokensNotExists, HttpStatusCode.BadRequest);
            }

            _unitOfWork.RefreshTokens.DeleteRange(tokens);
            _unitOfWork.SaveChanges();
        }


        private async Task<TokensPairModel> GenerateTokensPairAsync(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(
                user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name))
            );

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _unitOfWork.RefreshTokens.AddAsync(new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow + REFRESH_TOKEN_LIFETIME,
            });

            _unitOfWork.SaveChanges();

            return new TokensPairModel() 
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
