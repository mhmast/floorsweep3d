
using RobotActionType2 = FloorSweep.Engine.Core.RobotActionType;
using System;
using FloorSweep.Engine.Core;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class RobotActionDto 
    {

        public RobotActionDto(IRobotAction action)
        {
            Type = ConvertToRobotActionType(action.Type);
            Data = action.Data;
        }

        private static RobotActionType ConvertToRobotActionType(RobotActionType2 type)
        => type switch
        {
            RobotActionType2.Driving => RobotActionType.Driving,
            RobotActionType2.Turned => RobotActionType.Turned,
            RobotActionType2.Stopped => RobotActionType.Stopped,
            _ => throw new NotImplementedException(),
        };

        public RobotActionType Type { get; set; }

        public long Data { get; set; }
}
}
