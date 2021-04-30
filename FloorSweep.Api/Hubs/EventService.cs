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
        private readonly IStatusUpdateHandlerFactory<IRobotCommand> _robotCommandHandlerFactory;
        private readonly IStatusUpdateHandlerFactory<IRobotStatus> _robotStatusUpdateHandlerFactory;
        private readonly IStatusUpdateHandlerFactory<ILocationStatus> _locationStatusHandlerFactory;

        public EventService(
            IHubContext<EventService> context, 
            ISessionRepository sessionRepository, 
            IStatusUpdateHandlerFactory<IRobotCommand> robotCommandHandlerFactory,
            IStatusUpdateHandlerFactory<IRobotStatus> robotStatusUpdateHandlerFactory,
            IStatusUpdateHandlerFactory<ILocationStatus> locationStatusHandlerFactory)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _robotCommandHandlerFactory = robotCommandHandlerFactory;
            _robotStatusUpdateHandlerFactory = robotStatusUpdateHandlerFactory;
            _locationStatusHandlerFactory = locationStatusHandlerFactory;
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

        public Task SendRobotCommandAsync(IRobotCommand command)
        => Task.WhenAll(
            _robotCommandHandlerFactory.GetStatusUpdateHandler().OnStatusUpdatedAsync(command),
            NotifyRegisteredSignalRUsersAsync("OnRobotCommand", new[] { new RobotCommandDto(command) })
            );

        public Task SendLocationStatusUpdatedAsync(ILocationStatus locationStatus)
        => Task.WhenAll(
            _locationStatusHandlerFactory.GetStatusUpdateHandler().OnStatusUpdatedAsync(locationStatus),
            NotifyRegisteredSignalRUsersAsync("OnLocationStatusUpdated", new[] { new LocationStatusUpdateDto(locationStatus) })
            );

        public Task SendRobotStatusUpdateAsync(IRobotStatus status)
        => Task.WhenAll(
            _robotStatusUpdateHandlerFactory.GetStatusUpdateHandler().OnStatusUpdatedAsync(status),
            NotifyRegisteredSignalRUsersAsync("OnRobotStatusUpdated", new[] { new RobotStatusUpdateDto(status) })
            );
    }
}
