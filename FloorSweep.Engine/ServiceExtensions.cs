using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Core;
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
            .AddScoped<IMapService,MapService>()
            .AddScoped<IRobotCommandFactory,RobotCommandFactory>()
            .AddTransient<IDateTimeProvider,DateTimeProvider>()
            .AddTransient<IEventHandlerFactory<IRobotStatus>>(s=>new EventHandlerFactory<IRobotStatus>(s.GetRequiredService<ISessionRepository>,s.GetRequiredService<IMapService>))
            .AddTransient<IEventHandlerFactory<ILocationStatus>>(s=>new EventHandlerFactory<ILocationStatus>(s.GetRequiredService<ISessionRepository>))
            .AddTransient<IEventHandlerFactory<IRobotCommand>,EventHandlerFactory<IRobotCommand>>()
            .AddTransient<IEventHandlerFactory<IRobotCommand>,RobotCommandEventHandlerFactory>();

        
    }
}
