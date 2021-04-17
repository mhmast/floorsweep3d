using FloorSweep.Api.Interfaces;
using System;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class StatusUpdateActionDto
    {
        public StatusUpdateActionDto(IRobotAction robotAction)
        {
            Data = robotAction.Data;
            Type = ConvertToDtoType(robotAction.Type);
        }

        private static StatusUpdateRobotActionType ConvertToDtoType(RobotActionType type)
        => type switch
        {
            RobotActionType.Drive => StatusUpdateRobotActionType.Drive,
            RobotActionType.Turn => StatusUpdateRobotActionType.Turn,
            RobotActionType.Stop => StatusUpdateRobotActionType.Stop,
            _ => throw new NotImplementedException()
        };

        public int Data { get;  }
        public StatusUpdateRobotActionType Type { get; }
    }
}
