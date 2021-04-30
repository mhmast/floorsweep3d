using FloorSweep.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class RobotStatusUpdateDto
    {
        public RobotStatusUpdateDto(IRobotStatus status)
        {
            CurrentAction = new RobotActionDto(status.CurrentAction);
            DistanceToObject = status.DistanceToObject;
        }

        public RobotActionDto CurrentAction { get; }
        public int DistanceToObject { get; }
    }
}
