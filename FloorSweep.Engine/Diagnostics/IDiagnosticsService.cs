using FloorSweep.Engine.Models;
using FloorSweep.Engine.StatusHandlers;

namespace FloorSweep.Engine.Diagnostics
{
    public interface IDiagnosticsService : IStatusUpdateHandler<IRobotStatus>
    {
    }
}
