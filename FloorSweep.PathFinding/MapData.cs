using FloorSweep.Math;

namespace FloorSweep.PathFinding
{
    public class MapData
    {
        public Mat Map { get; set; }
        public Point Start { get; set; }
        public Point Target { get; set; }

        public Mat Image { get; set; }
    }
}
