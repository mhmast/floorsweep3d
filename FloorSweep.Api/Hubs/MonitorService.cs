using FloorSweep.Api.Hubs.Dtos;
using FloorSweep.Api.Interfaces;
using FloorSweep.Math;
using FloorSweep.PathFinding.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FloorSweep.Api.Hubs
{
    [Authorize]
    [AuthenticationFilter]
    [Scope("monitor-view")]
    public class MonitorService : Hub, IMonitorService
    {
        private readonly IHubContext<MonitorService> _context;
        private readonly ISessionRepository _sessionRepository;

        public MonitorService(IHubContext<MonitorService> context, ISessionRepository sessionRepository) 
        {
            _context = context;
            _sessionRepository = sessionRepository;
        }

        private async Task<IClientProxy> GetUserAsync()
        {
            var sessionId = (await _sessionRepository.GetSessionAsync())?.Id;
            return _context.Clients?.User(sessionId);
        }
        public async Task SendMatrixInitAsync(string name, Mat m, bool isBinary)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                _ = user.SendCoreAsync("OnMatrixInit", new[] { new MatrixInitDto(name, m, isBinary) });
            }
        }

        public async Task SendMatrixUpdateAsync(string name, int row, int col, double value)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                _ = user.SendCoreAsync("OnMatrixUpdate", new[] { new MatrixUpdateDto(name, row, col, value) });
            }
        }

        public async Task SendStatusChangedAsync(IRobotStatus status)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                _ = user.SendCoreAsync("OnStatusUpdated", new[] { new StatusUpdateDto(status) });
            }
        }
    }
}
