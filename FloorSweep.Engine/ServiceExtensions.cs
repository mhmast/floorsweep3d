using FloorSweep.Api;
using FloorSweep.Api.Interfaces;
using FloorSweep.Engine;
using FloorSweep.Engine.Factories;
using FloorSweep.Engine.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FloorSweep.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseFloorSweepEngine(this IServiceCollection collection)
        =>
            collection.AddTransient<IFloorSweepService, FloorSweepService>()
            .AddScoped<IMapService,MapService>()
            .AddScoped<IRobotCommandFactory,RobotCommandFactory>()
            .AddTransient<IDateTimeProvider,DateTimeProvider>()
            .AddTransient<IStatusUpdateHandlerFactory<IRobotStatus>>(s=>new RobotStatusUpdateHandlerFactory(s.GetRequiredService<ISessionRepository>,s.GetRequiredService<IMapService>))
            .AddTransient<IStatusUpdateHandlerFactory<ILocationStatus>>(s=>new LocationStatusUpdateHandlerFactory(s.GetRequiredService<ISessionRepository>))
            .AddTransient<IStatusUpdateHandlerFactory<IRobotCommand>,RobotCommandStatusUpdateHandlerFactory>();

        
    }
}
