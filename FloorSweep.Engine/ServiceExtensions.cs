using FloorSweep.Api;
using FloorSweep.Api.Interfaces;
using FloorSweep.Engine;
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
            .AddTransient(serviceProvider => Wrap<IRobotStatus>(serviceProvider.GetService<ISessionRepository>(), serviceProvider.GetService<IMapService>()))
            .AddTransient(serviceProvider => Wrap<ILocationStatus>(serviceProvider.GetService<ISessionRepository>()));

        static IStatusUpdateHandler<T> Wrap<T>(params IStatusUpdateHandler<T>[] handlers) => handlers.Aggregate((a, b) => new CompositeStatusHandler<T>(a, b));

        
    }
}
