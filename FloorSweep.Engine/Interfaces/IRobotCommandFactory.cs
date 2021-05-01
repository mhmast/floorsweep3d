namespace FloorSweep.Engine.Interfaces
{
    public interface IRobotCommandFactory
    {
        IRobotCommand CreateDriveCommand();
        IRobotCommand CreateTurnCommand(int v);
    }
}
