using FloorSweep.Api.Interfaces;
using System;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class RobotActionDto 
    {

        public RobotActionDto(IRobotAction action)
        {
            Type = ConvertToRobotActionType(action.Type);
            Data = action.Data;
        }

        private static RobotActionType ConvertToRobotActionType(Interfaces.RobotActionType type)
        => type switch
        {
            Interfaces.RobotActionType.Driving => RobotActionType.Driving,
            Interfaces.RobotActionType.Turned => RobotActionType.Turned,
            Interfaces.RobotActionType.Stopped => RobotActionType.Stopped,
            _ => throw new NotImplementedException(),
        };

        public RobotActionType Type { get; set; }

        public int Data { get; set; }
}
}
