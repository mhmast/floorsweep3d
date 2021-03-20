using FloorSweep.Engine;
using FloorSweep.Engine.Interfaces;
using FloorSweep.Engine.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FloorSweep.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseFloorSweepEngine(this IServiceCollection collection)
        =>
            collection.AddTransient<IFloorSweepService, FloorSweepService>()
            .AddTransient<IMonitorService, MonitorService>();
        
    }
}
