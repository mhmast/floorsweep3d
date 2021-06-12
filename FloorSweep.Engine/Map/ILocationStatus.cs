using FloorSweep.Math;

namespace FloorSweep.Engine.Map
{

    public interface ILocationStatus
    {
        public Point Location { get;  }
        public PointD Direction { get;  }
    }
}
