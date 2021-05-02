using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Api.Interfaces
{
    public interface IFloorSweepService
    {
        Task<IPath> FindPathAsync(IPathFindingParameters path);
        Task OnRobotStatusUpdatedAsync(IRobotStatus status);
        Task OnRobotStatusResetAsync(IRobotStatus status);
    }
}
