using RobotActionType2 = FloorSweep.Engine.Core.RobotActionType;
using System;
using FloorSweep.Engine.Core;

namespace FloorSweep.Api.Controllers.Models
{
    public class RobotActionDto : IRobotAction
    {
        public RobotActionType Type { get; set; }

        public long Data { get; set; }

        RobotActionType2 IRobotAction.Type => ConvertToDomainType(Type);

        private static RobotActionType2 ConvertToDomainType(RobotActionType type)
        => type switch
        {
            RobotActionType.Driving => RobotActionType2.Driving,
            RobotActionType.Turned => RobotActionType2.Turned,
            RobotActionType.Stopped => RobotActionType2.Stopped,
            _ => throw new NotImplementedException()
        };

        long IRobotAction.Data => Data;
    }
}
