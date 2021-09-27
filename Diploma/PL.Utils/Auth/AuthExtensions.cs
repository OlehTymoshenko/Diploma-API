using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace PL.Utils.Auth
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            var secret = configuration.GetValue<string>("AppSettings:Secret");

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config => 
            {
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    LifetimeValidator = (notBefore, expires, token, parameters) => 
                        notBefore <= DateTime.UtcNow && expires >= DateTime.UtcNow,

                    ClockSkew = TimeSpan.Zero
                };
                config.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = (authContext) =>
                    {
                        if (authContext.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            authContext.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Client, Policies.ClientAuthPolicy);
            });

            return services;
        }

    }
}
