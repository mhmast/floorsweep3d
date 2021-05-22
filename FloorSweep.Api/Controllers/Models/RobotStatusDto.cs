using FloorSweep.Engine.Models;
using System;

namespace FloorSweep.Api.Controllers.Models
{
    public class RobotStatusDto : IRobotStatus
    {
        public RobotActionDto CurrentAction { get; set; }

        public int DistanceToObject { get; set; }

        public DateTime StatusDate { get; set; }

        IRobotAction IRobotStatus.CurrentAction => CurrentAction;

        int IRobotStatus.DistanceToObject => DistanceToObject;
    }
}
