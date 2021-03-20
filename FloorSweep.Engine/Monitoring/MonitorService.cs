using FloorSweep.Engine.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Monitoring
{
    internal class MonitorService : IMonitorService
    {
        private readonly IMonitorRepository _monitorRepository;

        public MonitorService(IMonitorRepository monitorRepository)
        {
            _monitorRepository = monitorRepository;
        }

        public Task RegisterMonitorAsync(ISession session, string clientIP)
        => _monitorRepository.AddMonitorClientAsync(session, clientIP);
    }
}
