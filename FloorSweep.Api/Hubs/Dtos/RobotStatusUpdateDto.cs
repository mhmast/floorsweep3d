using FloorSweep.Engine.Models;

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
