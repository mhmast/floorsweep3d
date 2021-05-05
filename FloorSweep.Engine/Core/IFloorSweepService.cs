using FloorSweep.Engine.Models;
using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Core
{
    public interface IFloorSweepService
    {
        Task<IPath> FindPathAsync(IPathFindingParameters path);
        Task OnRobotStatusUpdatedAsync(IRobotStatus status);
        Task OnRobotStatusResetAsync(IRobotStatus status);
    }
}
