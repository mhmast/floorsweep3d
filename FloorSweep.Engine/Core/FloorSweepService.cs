using FloorSweep.Engine.Events;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Core
{
    internal class FloorSweepService : IFloorSweepService
    {
        private readonly IEventService _eventService;


        private readonly IMapService _mapService;

        public FloorSweepService(IEventService eventService,
            IMapService mapService)
        {
            _eventService = eventService;
            _mapService = mapService;
        }

        public async Task OnRobotStatusResetAsync(IRobotStatus status)
        {
            await _mapService.ResetStatusAsync();
            await _eventService.SendRobotStatusUpdateAsync(status);
        }
        public Task OnRobotStatusUpdatedAsync(IRobotStatus status)
        => _eventService.SendRobotStatusUpdateAsync(status);
     
    }
}
