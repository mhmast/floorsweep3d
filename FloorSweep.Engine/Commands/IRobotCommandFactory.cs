namespace FloorSweep.Engine.Commands
{
    public interface IRobotCommandFactory
    {
        IRobotCommand CreateDriveCommand();
        IRobotCommand CreateTurnCommand(int v);
    }
}
