using FloorSweep.Api.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine
{
    internal class StatusService : IStatusService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMonitorService _monitorService;

        public StatusService(ISessionRepository sessionRepository, IMonitorService monitorService)
        {
            _sessionRepository = sessionRepository;
            _monitorService = monitorService;
        }

        public async Task UpdateRobotStatusAsync(IRobotStatus status)
        {
            await _sessionRepository.UpdateStatusAsync(status);
            await _monitorService.SendStatusChangedAsync(status);
        }
    }
}
