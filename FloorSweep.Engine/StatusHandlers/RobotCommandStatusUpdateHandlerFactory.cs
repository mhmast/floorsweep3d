using FloorSweep.Engine.Commands;

namespace FloorSweep.Engine.StatusHandlers
{
    internal class RobotCommandStatusUpdateHandlerFactory : IStatusUpdateHandlerFactory<IRobotCommand>
    {

        public IStatusUpdateHandler<IRobotCommand> GetStatusUpdateHandler()
        => new EmptyStatusHandler<IRobotCommand>();
    }
}
