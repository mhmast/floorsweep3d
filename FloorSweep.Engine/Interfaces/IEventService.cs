using FloorSweep.Engine.Interfaces;
using FloorSweep.Math;
using System;
using System.Threading.Tasks;

namespace FloorSweep.Api.Interfaces
{
    public interface IEventService
    {
        Task SendMatrixInitAsync(string name,Mat m,bool isBinary);
        Task SendMatrixUpdateAsync(string key, int row, int col, double value);
        Task SendRobotStatusUpdatedAsync(IRobotStatus status);
        Task SendLocationStatusUpdatedAsync(ILocationStatus locationStatus);
    }
}