using FloorSweep.Engine.Commands;
using RobotCommandType2 = FloorSweep.Engine.Commands.RobotCommandType;
using System;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class RobotCommandDto
    {
        public RobotCommandDto(IRobotCommand command)
        {
            Data = command.Data;
            Type = ConvertToDtoType(command.Type);
        }

        private static RobotCommandType ConvertToDtoType(RobotCommandType2 type)
        => type switch
        {
            RobotCommandType2.Drive => RobotCommandType.Drive,
            RobotCommandType2.Turn => RobotCommandType.Turn,
            RobotCommandType2.Stop => RobotCommandType.Stop,
            _ => throw new NotImplementedException()
        };

        public double Data { get;  }
        public RobotCommandType Type { get; }
    }
}
