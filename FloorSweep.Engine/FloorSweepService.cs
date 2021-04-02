using FloorSweep.Engine.Interfaces;
using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.Engine
{
    internal class FloorSweepService : IFloorSweepService
    {
        private readonly IMonitorService _monitorService;

        public FloorSweepService(IMonitorService monitorService)
        {
            _monitorService = monitorService;
        }
        public Task<IPath> FindPathAsync(ISession session, IPathFindingParameters parameters)

            => session.PathFindingSession.FindPathAsync(parameters.Start, parameters.Target,SendInitToMonitor);

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
