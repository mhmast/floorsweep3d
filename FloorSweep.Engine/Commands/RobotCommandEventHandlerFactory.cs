using FloorSweep.Engine.EventHandlers;

namespace FloorSweep.Engine.Commands
{
    internal class RobotCommandEventHandlerFactory : IEventHandlerFactory<IRobotCommand>
    {

        public IEventHandler<IRobotCommand> GetEventHandler()
        => new EmptyEventHandler<IRobotCommand>();
    }
}
