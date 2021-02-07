using OpenCvSharp;
using System.Collections.Generic;

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
        public Mat[] Graph { get; internal set; }
        public double KM { get;  set; }
        public SortedSet<Mat> Stack { get; internal set; }
        public bool Exist { get; internal set; }
        public double Length { get; internal set; }
    }
}
