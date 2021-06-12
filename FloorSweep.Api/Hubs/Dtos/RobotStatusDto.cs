using FloorSweep.Engine.Core;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class RobotStatusDto
    {
        public RobotStatusDto(IRobotStatus status)
        {
            CurrentAction = new RobotActionDto(status.CurrentAction);
            DistanceToObject = status.DistanceToObject;
        }

        public RobotActionDto CurrentAction { get; }
        public int DistanceToObject { get; }
    }
}
