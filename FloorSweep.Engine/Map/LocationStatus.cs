using FloorSweep.Engine.Core;
using FloorSweep.Math;

namespace FloorSweep.Engine.Map
{

    internal class LocationStatus : ILocationStatus
    {

        public Point Location { get; internal set; }
        public PointD Direction => PointD.Up * Rotation;

        public IRobotStatus LastReceivedStatus { get; set; }
        public Mat Rotation => Mat.Rotate(RotationDegrees);
        public int RotationDegrees { get; internal set; }
    }
}