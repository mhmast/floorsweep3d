using FloorSweep.Engine.Events;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Core
{
    internal class FloorSweepService : IFloorSweepService
    {
        private readonly IEventService _eventService;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapService _mapService;

        public FloorSweepService(IEventService eventService, 
            ISessionRepository sessionRepository,
            IMapService mapService)
        {
            _eventService = eventService;
            _sessionRepository = sessionRepository;
            _mapService = mapService;
        }
        public async Task<IPath> FindPathAsync(IPathFindingParameters parameters)
        {
            var session = await _sessionRepository.GetSessionAsync();
            return await session.GetObject<IPathFindingSession>().FindPathAsync(parameters.Start, parameters.Target, SendInitToMonitor);
        }

        public async Task OnRobotStatusResetAsync(IRobotStatus status)
        {
            await _mapService.ResetStatusAsync();
            await _eventService.SendRobotStatusUpdateAsync(status);
        }
        public Task OnRobotStatusUpdatedAsync(IRobotStatus status)
        =>
            _eventService.SendRobotStatusUpdateAsync(status);


        private async Task SendInitToMonitor(IReadOnlyDictionary<string, Mat> matrices, IReadOnlyDictionary<string, bool> matrixBitness)
        {
            foreach (var mat in matrices)
            {
                mat.Value.MatChanged += (row, col, value) => _eventService.SendMatrixUpdateAsync(mat.Key, row, col, value);
                await _eventService.SendMatrixInitAsync(mat.Key, mat.Value, matrixBitness[mat.Key]);
            }
        }


    }
}
