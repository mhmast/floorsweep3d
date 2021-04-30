namespace FloorSweep.Engine.Interfaces
{
    public interface IRobotCommand
    {
        RobotCommandType Type { get; }
        double Data { get; }
    }
}
