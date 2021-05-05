using FloorSweep.Api.Hubs;
using FloorSweep.Api.Repositories;
using FloorSweep.Engine;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Session;
using FloorSweep.PathFinding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag.Generation.Processors.Security;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FloorSweep.Api
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
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/hubs"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };

                });
            services.AddHttpContextAccessor();
            services.AddSingleton<ISessionRepository, SessionRepository>();
            services.AddAuthorization();
            services.AddControllers()
                .AddJsonOptions(o=>o.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals);
            services.AddCors(o =>
            {
                var policy = new CorsPolicy();
                Configuration.GetSection("Cors").Bind(policy);
                o.AddDefaultPolicy(policy);
            });
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
            //services.UseFloorSweepRepositories();
            services.UseFocussedDStar();
            services.AddSignalR().AddJsonProtocol(o=>o.PayloadSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals);
            services.AddTransient<IUserIdProvider, UserIdProvider>();
            services.AddTransient<IEventService, EventService>();
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
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<EventService>("/hubs/monitor");
            });
        }
    }
}
