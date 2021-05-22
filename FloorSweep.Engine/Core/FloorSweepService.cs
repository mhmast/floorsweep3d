using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Core
{
    internal class FloorSweepService : IFloorSweepService
    {
        private readonly IEventService _eventService;


        private readonly IEventHandlerFactory<IRobotStatus> _factory;

        public FloorSweepService(IEventService eventService,
           IEventHandlerFactory<IRobotStatus> factory)
        {
            _eventService = eventService;
            _factory = factory;
        }

        public async Task OnRobotStatusResetAsync(IRobotStatus status)
        {
            await _factory.GetEventHandler().ResetStatusAsync();
            await _eventService.SendRobotStatusUpdateAsync(status);
        }
        public Task OnRobotStatusUpdatedAsync(IRobotStatus status)
        => _eventService.SendRobotStatusUpdateAsync(status);

    }
}
