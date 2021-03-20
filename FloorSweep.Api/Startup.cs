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
using System;
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
                .AddJwtBearer(o => authSection.Bind(o));
            services.AddHttpContextAccessor();
            services.AddTransient<ISessionFactory, HttpSessionFactory>();
            services.AddSwaggerGen(c =>
            {
                var flow = new OpenApiOAuthFlow();
                authSection.Bind(flow);
                var scheme = new OpenApiSecurityScheme {Flows = new OpenApiOAuthFlows { AuthorizationCode=flow} ,Type = SecuritySchemeType.OpenIdConnect};
                authSection.Bind(scheme);
                c.AddSecurityDefinition("oauth2", scheme);
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FloorSweep.Api", Version = "v1" });
            });

            services.UseFloorSweepEngine();
            services.UseFloorSweepRepositories();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var authSection = Configuration.GetSection("Authentication");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FloorSweep.PathFinding.Api v1");
                    authSection.Bind(c.OAuthConfigObject);
                    c.OAuthAppName("Demo API - Swagger");
                    c.OAuthUsePkce();
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
