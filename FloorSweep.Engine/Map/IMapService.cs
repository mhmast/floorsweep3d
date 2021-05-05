using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Models;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Map
{
    public interface IMapService : IEventHandler<IRobotStatus>
    {
        Task ResetStatusAsync();
    }
}
