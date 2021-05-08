using FloorSweep.Api.Hubs.Dtos;
using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Diagnostics;
using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using FloorSweep.Math;
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
        private readonly IEventHandlerFactory<IRobotCommand> _robotCommandHandlerFactory;
        private readonly IEventHandlerFactory<IRobotStatus> _robotStatusUpdateHandlerFactory;
        private readonly IEventHandlerFactory<ILocationStatus> _locationStatusHandlerFactory;
        private readonly IEventHandlerFactory<IDiagnosticStatusData> _diagnosticStatusHandlerFactory;

        public EventService(
            IHubContext<EventService> context, 
            ISessionRepository sessionRepository, 
            IEventHandlerFactory<IRobotCommand> robotCommandHandlerFactory,
            IEventHandlerFactory<IRobotStatus> robotStatusUpdateHandlerFactory,
            IEventHandlerFactory<ILocationStatus> locationStatusHandlerFactory,
            IEventHandlerFactory<IDiagnosticStatusData> diagnosticStatusHandlerFactory)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _robotCommandHandlerFactory = robotCommandHandlerFactory;
            _robotStatusUpdateHandlerFactory = robotStatusUpdateHandlerFactory;
            _locationStatusHandlerFactory = locationStatusHandlerFactory;
            _diagnosticStatusHandlerFactory = diagnosticStatusHandlerFactory;
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
            _robotCommandHandlerFactory.GetEventHandler().OnStatusUpdatedAsync(command),
            NotifyRegisteredSignalRUsersAsync("OnRobotCommand", new[] { new RobotCommandDto(command) })
            );

        public Task SendLocationStatusUpdatedAsync(ILocationStatus locationStatus)
        => Task.WhenAll(
            _locationStatusHandlerFactory.GetEventHandler().OnStatusUpdatedAsync(locationStatus),
            NotifyRegisteredSignalRUsersAsync("OnLocationStatusUpdated", new[] { new LocationStatusUpdateDto(locationStatus) })
            );
        
        public Task SendDiagnosticStatusUpdatedAsync(IDiagnosticStatusData diagnosticStatus)
        => Task.WhenAll(
            _diagnosticStatusHandlerFactory.GetEventHandler().OnStatusUpdatedAsync(diagnosticStatus),
            NotifyRegisteredSignalRUsersAsync("OnDiagnosticStatusUpdated", new[] { new DiagnosticStatusUpdateDto(diagnosticStatus) })
            );

        public Task SendRobotStatusUpdateAsync(IRobotStatus status)
        => Task.WhenAll(
            _robotStatusUpdateHandlerFactory.GetEventHandler().OnStatusUpdatedAsync(status),
            NotifyRegisteredSignalRUsersAsync("OnRobotStatusUpdated", new[] { new RobotStatusUpdateDto(status) })
            );
    }
}
