using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Config;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.Diagnostics;
using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FloorSweep.Engine
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseFloorSweepEngine(this IServiceCollection collection,IConfiguration config)
        {
            var mapConfig = new MapConfiguration();
            config.GetSection("Map").Bind(mapConfig);
            return collection.AddTransient<IFloorSweepService, FloorSweepService>()
            .AddScoped<IMapService, MapService>()
            .AddScoped<IDiagnosticService, DiagnosticService>()
            .AddScoped<IRobotCommandFactory, RobotCommandFactory>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>()
            .AddTransient(s => EventHandlerFactory<IRobotStatus>.Builder
            .WithInterceptors(() => new EventHandlerAdapter<IRobotStatus>(s.GetRequiredService<ISessionRepository>().SaveObjectAsync, () => Task.CompletedTask))
            .WithDecorators(s.GetRequiredService<IDiagnosticService>, s.GetRequiredService<IMapService>
            )
            .Build()
            )
            .AddTransient(s => EventHandlerFactory<ILocationStatus>.Builder
            .Build())
            .AddTransient(s => EventHandlerFactory<IRobotCommand>.Builder.Build())
            .AddTransient(s => EventHandlerFactory<IDiagnosticStatusData>.Builder
            .Build()
            )
            .AddTransient<IMapConfiguration>(_=>mapConfig);
        }
    }
}
