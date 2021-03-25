using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PL.Utils.Swagger;
using PL.Utils.Auth;
using Common.Configurations;
using DL.DIExtesion;
using BL.DIExtension;
using PL.Utils.ExceptionsHandler;

namespace PL.Diploma.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppConfigurationSections(Configuration);

            services.AddDataLayer(Configuration);
            services.AddBusinessLayer();
            
            services.AddCors();
            
            services.AddControllers();

            services.AddCustomAuthentication(Configuration);
            services.AddCustomAuthorization();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddSwaggerDocs(xmlPath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> _logger)
        {
            if (env.IsDevelopment())
            {
                _logger.LogDebug("Configure development environment...");

                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerDocs(); // propably it shold be moved to upper block IsDevelopment
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin().
                                           AllowAnyMethod().
                                           AllowAnyHeader());
            
            app.UseCustomExceptionsHandlerMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
