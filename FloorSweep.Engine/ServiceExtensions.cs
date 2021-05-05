using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using FloorSweep.Engine.StatusHandlers;
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
            .AddTransient<IStatusUpdateHandlerFactory<IRobotStatus>>(s=>new RobotStatusUpdateHandlerFactory(s.GetRequiredService<ISessionRepository>,s.GetRequiredService<IMapService>))
            .AddTransient<IStatusUpdateHandlerFactory<ILocationStatus>>(s=>new LocationStatusUpdateHandlerFactory(s.GetRequiredService<ISessionRepository>))
            .AddTransient<IStatusUpdateHandlerFactory<IRobotCommand>,RobotCommandStatusUpdateHandlerFactory>();

        
    }
}
