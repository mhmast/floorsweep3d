using FloorSweep.Engine;
using FloorSweep.Engine.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FloorSweep.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseFloorSweepEngine(this IServiceCollection collection)
        =>
            collection.AddTransient<IFloorSweepService, FloorSweepService>();

    }
}
