using OpenCvSharp;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    public class State
    {
        private Mat _map;

        public Mat Map
        {
            get { return _map; }
            set
            {
                _map = value;
                Vis = Mat.Zeros(rows: Height, Width, MatType.CV_64F);
                Template = Mat.Zeros(rows: Height, Width, MatType.CV_64F);
            }
        }
        public Mat StartPos { get; internal set; }
        public Mat EndPos { get; internal set; }
        public int Scaling { get; internal set; }
        public Mat Pattern { get; internal set; }
        public Mat Ucc { get; internal set; }
        public int Height { get => Map.Height; }
        public int Width { get => Map.Width; }
        public Mat[] Graph { get; internal set; }
        public double KM { get; set; }
        public SortedSet<Mat> Stack { get; internal set; }
        public bool Exist { get; internal set; }
        public double Length { get; internal set; }
        public Mat Path { get; set; }
        public Mat Vis { get; internal set; }
        public Mat Template { get; internal set; }
        public Mat Image { get; internal set; }
    }
}
