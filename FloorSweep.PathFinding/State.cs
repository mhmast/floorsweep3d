using OpenCvSharp;

namespace FloorSweep.PathFinding
{
    public class State
    {
        public Mat Map { get; internal set; }
        public Mat StartPos { get; internal set; }
        public Mat EndPos { get; internal set; }
        public int Scaling { get; internal set; }
        public Mat Pattern { get; internal set; }
        public Mat Ucc { get; internal set; }
        public int Height { get; internal set; }
        public int Width { get; internal set; }
    }
}
