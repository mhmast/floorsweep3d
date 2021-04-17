using System.Threading.Tasks;

namespace FloorSweep.Api.Interfaces
{
    public interface IStatusService
    {
        Task UpdateRobotStatusAsync(IRobotStatus status);
    }
}
