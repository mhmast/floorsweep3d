using FloorSweep.Engine.Core;
using FloorSweep.Engine.EventHandlers;


namespace FloorSweep.Engine.Diagnostics
{
    public interface IDiagnosticService : IEventHandlerDecorator<IRobotStatus>
    {
    }
}
