using System.Threading.Tasks;

namespace FloorSweep.Api.Interfaces
{
    public interface IStatusHandler
    {
        Task HandleStatusChangedAsync(IRobotStatus status);
    }
}