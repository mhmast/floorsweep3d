using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.PathFinding
{
    internal class Node : IQueueKeyProvider
    {

        public Node(Point xy, PointD ab, Point targetPos)
        {
            XY = xy;
            AB = ab;
            Key = System.Math.Abs((long)xy.Length - targetPos.Length) << 32;
            Key += xy.X;
        }

        public Point XY { get; set; }
        public PointD AB { get; set; }

        public long Key { get; }
    }
}
