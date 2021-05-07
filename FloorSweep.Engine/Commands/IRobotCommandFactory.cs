namespace FloorSweep.Engine.Commands
{
    public interface IRobotCommandFactory
    {
        IRobotCommand CreateDriveCommand();
        IRobotCommand CreateStopCommand();
        IRobotCommand CreateTurnCommand(int degrees);
    }
}
