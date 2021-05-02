using FloorSweep.Api.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IMapService : IStatusUpdateHandler<IRobotStatus>
    {
        Task ResetStatusAsync();
    }
}
