using FloorSweep.Engine.Map;
using FloorSweep.Math;

namespace FloorSweep.Api.Hubs.Dtos
{
    internal class LocationStatusUpdateDto
    {
        public LocationStatusUpdateDto(ILocationStatus status)
        {
            Direction = status.Direction;
            LastReceivedStatus = new RobotStatusDto(status.LastReceivedStatus);
            Location = status.Location;
            RotationDegrees = status.RotationDegrees;
        }

        public PointD Direction { get; }
        public RobotStatusDto LastReceivedStatus { get; }
        public Point Location { get; }
        public int RotationDegrees { get; }
    }
}
