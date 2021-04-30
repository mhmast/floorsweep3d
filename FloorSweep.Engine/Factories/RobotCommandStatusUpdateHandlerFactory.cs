using FloorSweep.Engine.Interfaces;

namespace FloorSweep.Engine.Factories
{
    internal class RobotCommandStatusUpdateHandlerFactory : IStatusUpdateHandlerFactory<IRobotCommand>
    {

        public IStatusUpdateHandler<IRobotCommand> GetStatusUpdateHandler()
        => new EmptyStatusHandler<IRobotCommand>();
    }
}
