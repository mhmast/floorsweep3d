using OpenCvSharp;

namespace FloorSweep.PathFinding
{
    public class MapData
    {
        public Mat Map { get; set; }
        public Mat Start { get; set; }
        public Mat Target { get; set; }
    }
}
