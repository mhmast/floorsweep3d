using FloorSweep.Api.Interfaces;
using System;

namespace FloorSweep.Api.Controllers.Models
{
    public class RobotActionDto : IRobotAction
    {
        public RobotActionType Type { get; set; }

        public int Data { get; set; }

        Api.Interfaces.RobotActionType IRobotAction.Type => ConvertToDomainType(Type);

        private static Api.Interfaces.RobotActionType ConvertToDomainType(RobotActionType type)
        => type switch
        {
            RobotActionType.Drive => Api.Interfaces.RobotActionType.Drive,
            RobotActionType.Turn => Api.Interfaces.RobotActionType.Turn,
            RobotActionType.Stop => Api.Interfaces.RobotActionType.Stop,
            _ => throw new NotImplementedException()
        };

        int IRobotAction.Data => Data;
    }
}
