using FloorSweep.Api.Interfaces;
using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.Api
{
    internal class FloorSweepService : IFloorSweepService
    {
        private readonly IMonitorService _monitorService;
        private readonly ISessionRepository _sessionRepository;

        public FloorSweepService(IMonitorService monitorService, ISessionRepository sessionRepository)
        {
            _monitorService = monitorService;
            _sessionRepository = sessionRepository;
        }
        public async Task<IPath> FindPathAsync(IPathFindingParameters parameters)
        {
            var session = await _sessionRepository.GetSessionAsync();
            return await session.PathFindingSession.FindPathAsync(parameters.Start, parameters.Target, SendInitToMonitor);
        }

        private async Task SendInitToMonitor(IReadOnlyDictionary<string, Mat> matrices, IReadOnlyDictionary<string, bool> matrixBitness)
        {
            foreach(var mat in matrices)
            {
                mat.Value.MatChanged += (row, col, value) => _monitorService.SendMatrixUpdateAsync(mat.Key, row, col, value);
                await _monitorService.SendMatrixInitAsync(mat.Key, mat.Value, matrixBitness[mat.Key]);
            }
        }

       
    }
}
