using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.Diagnostics;
using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using Microsoft.Extensions.DependencyInjection;

namespace FloorSweep.Engine
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseFloorSweepEngine(this IServiceCollection collection)
        =>
            collection.AddTransient<IFloorSweepService, FloorSweepService>()
            .AddScoped<IMapService, MapService>()
            .AddScoped<IDiagnosticService, DiagnosticService>()
            .AddScoped<IRobotCommandFactory, RobotCommandFactory>()
            .AddTransient<IDateTimeProvider, DateTimeProvider>()
            .AddTransient(s => EventHandlerFactory<IRobotStatus>.Builder
                .WithInterceptors(s.GetRequiredService<ISessionRepository>().SaveObjectAsync)
                .WithDecorators(
                    status => s.GetRequiredService<IDiagnosticService>().OnStatusUpdatedAsync(status),
                    status => s.GetRequiredService<IMapService>().OnStatusUpdatedAsync(status)
                )
                .Build()
            )
            .AddTransient(s => EventHandlerFactory<ILocationStatus>.Builder
                .Build())
            .AddTransient(s => EventHandlerFactory<IRobotCommand>.Builder.Build())
            .AddTransient(s => EventHandlerFactory<IDiagnosticStatusData>.Builder
                .Build()
            );


    }
}
