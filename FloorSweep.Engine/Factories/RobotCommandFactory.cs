using FloorSweep.Engine.Interfaces;

namespace FloorSweep.Engine.Factories
{
    internal class RobotCommandFactory : IRobotCommandFactory
    {
        class RobotCommand : IRobotCommand
        {
            public RobotCommandType Type { get; set; }

            public double Data {get;set;}
        }
        public IRobotCommand CreateDriveCommand()
        => new RobotCommand { Type = RobotCommandType.Drive };

        public IRobotCommand CreateTurnCommand(int degrees)
        => new RobotCommand { Type = RobotCommandType.Turn, Data = degrees };
    }
}
