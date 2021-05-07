using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Models;

namespace FloorSweep.Engine.Diagnostics
{
    public interface IDiagnosticService : IEventHandlerDecorator<IRobotStatus>
    {
    }
}
