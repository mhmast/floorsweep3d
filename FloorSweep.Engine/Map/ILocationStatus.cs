using FloorSweep.Engine.Core;
using FloorSweep.Math;

namespace FloorSweep.Engine.Map
{

    public interface ILocationStatus
    {
        PointD Direction { get; }
        IRobotStatus LastReceivedStatus { get;  }
        Point Location { get; }
        int RotationDegrees { get; }
    }
}
