namespace FloorSweep.Engine.Commands
{
    public interface IRobotCommand
    {
        RobotCommandType Type { get; }
        double Data { get; }
    }
}
