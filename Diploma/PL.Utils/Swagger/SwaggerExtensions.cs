using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace PL.Utils.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v0.1", new OpenApiInfo
                {
                    Version = "v0.1",
                    Title = "Diploma API",
                    Description = "Diploma API description.\n This is a system for automatic " +
                        "generation files for scientific publications"
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, Array.Empty<string>() }
                });

                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseSwagger();

            appBuilder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v0.1/swagger.json", "DIPLOMA API V0.1");
            });

            return appBuilder;
        }
    }
}
