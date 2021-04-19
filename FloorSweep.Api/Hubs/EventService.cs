using FloorSweep.Api.Hubs.Dtos;
using FloorSweep.Api.Interfaces;
using FloorSweep.Engine.Interfaces;
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
    public class EventService : Hub, IEventService
    {
        private readonly IHubContext<EventService> _context;
        private readonly ISessionRepository _sessionRepository;
        private readonly IStatusUpdateHandler<IRobotStatus> _robotStatusHandler;
        private readonly IStatusUpdateHandler<ILocationStatus> _locationStatusandler;

        public EventService(
            IHubContext<EventService> context, 
            ISessionRepository sessionRepository, 
            IStatusUpdateHandler<IRobotStatus> robotStatusandler,
            IStatusUpdateHandler<ILocationStatus> locationStatusandler)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _robotStatusHandler = robotStatusandler;
            _locationStatusandler = locationStatusandler;
        }

        private async Task<IClientProxy> GetUserAsync()
        {
            var sessionId = (await _sessionRepository.GetSessionAsync())?.Id;
            return _context.Clients?.User(sessionId);
        }

        private async Task NotifyRegisteredSignalRUsersAsync(string @event, params object[] args)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                await user.SendCoreAsync(@event, args);
            }
        }
        public Task SendMatrixInitAsync(string name, Mat m, bool isBinary)
        => NotifyRegisteredSignalRUsersAsync("OnMatrixInit", new[] { new MatrixInitDto(name, m, isBinary) });

        public Task SendMatrixUpdateAsync(string name, int row, int col, double value)
        => NotifyRegisteredSignalRUsersAsync("OnMatrixUpdate", new[] { new MatrixUpdateDto(name, row, col, value) });

        public Task SendRobotStatusUpdatedAsync(IRobotStatus status)
        => Task.WhenAll(
            _robotStatusHandler.OnStatusUpdatedAsync(status),
            NotifyRegisteredSignalRUsersAsync("OnRobotStatusUpdated", new[] { new RobotStatusUpdateDto(status) })
            );

        public Task SendLocationStatusUpdatedAsync(ILocationStatus locationStatus)
        => Task.WhenAll(
            _locationStatusandler.OnStatusUpdatedAsync(locationStatus),
            NotifyRegisteredSignalRUsersAsync("OnLocationStatusUpdated", new[] { new LocationStatusUpdateDto(locationStatus) })
            );

    }
}
