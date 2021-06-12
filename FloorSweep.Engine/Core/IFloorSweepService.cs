using System.Threading.Tasks;

namespace FloorSweep.Engine.Core
{
    public interface IFloorSweepService
    {
        Task OnRobotStatusUpdatedAsync(IRobotStatus status);
        Task OnRobotStatusResetAsync(IRobotStatus status);
    }
}
