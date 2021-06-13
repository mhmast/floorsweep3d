using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Config;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.Diagnostics;
using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace FloorSweep.Engine
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseFloorSweepEngine(this IServiceCollection collection, IConfiguration config)
        {
            var mapConfig = new MapConfiguration();
            config.GetSection("Map").Bind(mapConfig);
            return collection.AddTransient<IFloorSweepService, FloorSweepService>()
            .AddScoped<IMapService, MapService>()
            .AddScoped<IDiagnosticService, DiagnosticService>()
            .AddScoped<IRobotCommandFactory, RobotCommandFactory>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>()
            .AddTransient(s => EventHandlerFactory<IRobotStatus>.Builder
                .WithInterceptors(s.GetRequiredService<IMapService>, () => new EventHandlerAdapter<IRobotStatus>(s.GetRequiredService<ISessionRepository>().SaveObjectAsync))
                .WithDecorators(s.GetRequiredService<IDiagnosticService>)
                .Build()
                )
            .AddTransient<IDataProvider<IRobotMeta>,DiagnosticService>()
            .AddTransient(s => EventHandlerFactory<IRobotCommand>.Builder.Build())
            .AddTransient(s => EventHandlerFactory<ISession>.Builder.Build())
            .AddTransient<IMapConfiguration>(_ => mapConfig);
        }
    }
}
