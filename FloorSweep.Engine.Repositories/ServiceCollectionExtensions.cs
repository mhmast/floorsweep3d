using FloorSweep.Engine.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FloorSweep.Engine.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseFloorSweepRepositories(this IServiceCollection collection) 
            => collection.AddSingleton<ISessionRepository, SessionRepository>()
           .AddSingleton<IMonitorRepository, MonitorRepository>();
    }
}
