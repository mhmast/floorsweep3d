using FloorSweep.Api;
using FloorSweep.Engine.Repositories;
using FloorSweep.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NSwag.Generation.Processors.Security;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Api
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
            var authSection = Configuration.GetSection("Authentication");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
#if DEBUG
                    o.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true };
#endif
                    authSection.Bind(o);
                    
                });
            services.AddHttpContextAccessor();
            services.AddTransient<ISessionFactory, HttpSessionFactory>();
            services.AddAuthorization();
            services.AddControllers();
            services.AddOpenApiDocument(c =>
            {
                var section = Configuration.GetSection("Swagger");
                var flow = new NSwag.OpenApiOAuthFlow();
                section.Bind(flow);
                var scheme = new NSwag.OpenApiSecurityScheme {Flows = new NSwag.OpenApiOAuthFlows { AuthorizationCode=flow} ,Type = NSwag.OpenApiSecuritySchemeType.OAuth2};
                section.Bind(scheme);
                c.AddSecurity("oauth2", scheme);
                c.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
                c.Title = "FloorSweep.Api";
                c.Version = "v1";
            });

            services.UseFloorSweepEngine();
            services.UseFloorSweepRepositories();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var swaggerSection = Configuration.GetSection("Swagger");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3(options =>
                {
                    options.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings();
                    swaggerSection.Bind(options.OAuth2Client); 
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
