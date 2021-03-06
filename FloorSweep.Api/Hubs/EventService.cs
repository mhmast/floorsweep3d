﻿using FloorSweep.Api.Hubs.Dtos;
using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Core;
using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Session;
using FloorSweep.Math;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FloorSweep.Api.Hubs
{
    [Authorize]
    [AuthenticationFilter]
    [Scope("monitor-view")]
    public class EventService : Hub, IEventService
    {
        private readonly IHubContext<EventService> _context;
        private readonly UserIdProvider _userIdProvider;
        private readonly IEventHandlerFactory<IRobotCommand> _robotCommandHandlerFactory;
        private readonly IEventHandlerFactory<ISession> _sessionStatusUpdateHandler;
        private readonly IEventHandlerFactory<IRobotStatus> _robotStatusUpdateHandlerFactory;


        public EventService(
            IHubContext<EventService> context,
            UserIdProvider userIdProvider,
            IEventHandlerFactory<IRobotCommand> robotCommandHandlerFactory,
            IEventHandlerFactory<ISession> sessionStatusUpdateHandler,
            IEventHandlerFactory<IRobotStatus> robotStatusUpdateHandlerFactory)
        {
            _context = context;
            _userIdProvider = userIdProvider;
            _robotCommandHandlerFactory = robotCommandHandlerFactory;
            _sessionStatusUpdateHandler = sessionStatusUpdateHandler;
            _robotStatusUpdateHandlerFactory = robotStatusUpdateHandlerFactory;
        }

        private IClientProxy GetUser() => _context.Clients?.User(_userIdProvider.GetUserId());

        private async Task NotifyRegisteredSignalRUsersAsync(string @event, params object[] args)
        {
            var user = GetUser();
            if (user != null)
            {
                await user.SendCoreAsync(@event, args);
            }
        }
        public Task SendMatrixInitAsync(string name, Mat m, bool isBinary)
        => NotifyRegisteredSignalRUsersAsync("OnMatrixInit", new MatrixInitDto(name, m, isBinary) );

        public Task SendMatrixUpdateAsync(string name, int row, int col, double value)
        => NotifyRegisteredSignalRUsersAsync("OnMatrixUpdate", new MatrixUpdateDto(name, row, col, value) );

        public Task SendRobotCommandAsync(IRobotCommand command)
        => Task.WhenAll(
            _robotCommandHandlerFactory.GetEventHandler().OnStatusUpdatedAsync(command),
            NotifyRegisteredSignalRUsersAsync("OnRobotCommand",  new RobotCommandDto(command) )
            );


        public Task SendRobotStatusUpdateAsync(IRobotStatus status)
        => Task.WhenAll(
            _robotStatusUpdateHandlerFactory.GetEventHandler().OnStatusUpdatedAsync(status),
            NotifyRegisteredSignalRUsersAsync("OnRobotStatusUpdated", new RobotStatusDto(status) )
            );

        
        public Task SendSessionUpdatedAsync(ISession session)
         => Task.WhenAll(
            _sessionStatusUpdateHandler.GetEventHandler().OnStatusUpdatedAsync(session),
            NotifyRegisteredSignalRUsersAsync("OnSessionUpdated", session)
            );
    }
}
