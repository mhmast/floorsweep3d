using FloorSweep.Engine.Interfaces;
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

        private static RobotCommandType ConvertToDtoType(Engine.Interfaces.RobotCommandType type)
        => type switch
        {
            Engine.Interfaces.RobotCommandType.Drive => RobotCommandType.Drive,
            Engine.Interfaces.RobotCommandType.Turn => RobotCommandType.Turn,
            Engine.Interfaces.RobotCommandType.Stop => RobotCommandType.Stop,
            _ => throw new NotImplementedException()
        };

        public double Data { get;  }
        public RobotCommandType Type { get; }
    }
}
