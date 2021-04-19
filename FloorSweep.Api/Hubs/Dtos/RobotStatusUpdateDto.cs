using FloorSweep.Api.Interfaces;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class RobotStatusUpdateDto
    {
        public RobotStatusUpdateDto(IRobotStatus status)
        {
            DistanceToObject = status.DistanceToObject;
            CurrentAction = new StatusUpdateActionDto(status.CurrentAction);
        }

        public int DistanceToObject { get; }
        public StatusUpdateActionDto CurrentAction { get;  }
    }
}
