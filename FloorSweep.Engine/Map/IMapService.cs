using FloorSweep.Engine.Models;
using FloorSweep.Engine.StatusHandlers;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Map
{
    public interface IMapService : IStatusUpdateHandler<IRobotStatus>
    {
        Task ResetStatusAsync();
    }
}
