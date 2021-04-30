using FloorSweep.Api.Interfaces;
using System;

namespace FloorSweep.Api.Controllers.Models
{
    public class RobotActionDto : IRobotAction
    {
        public RobotActionType Type { get; set; }

        public int Data { get; set; }

        Interfaces.RobotActionType IRobotAction.Type => ConvertToDomainType(Type);

        private static Api.Interfaces.RobotActionType ConvertToDomainType(RobotActionType type)
        => type switch
        {
            RobotActionType.Driving => Interfaces.RobotActionType.Driving,
            RobotActionType.Turned => Interfaces.RobotActionType.Turned,
            RobotActionType.Stopped => Interfaces.RobotActionType.Stopped,
            _ => throw new NotImplementedException()
        };

        int IRobotAction.Data => Data;
    }
}
