using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    public class State
    {
        private Mat _map;
        private List<Mat> _path = new List<Mat>();

        public event Action PathFound;
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
        public List<Mat> Path
        {
            get => _path; 
            set
            {
                _path = value;
                PathFound?.Invoke();
            }
        }
        public Mat Vis { get; internal set; }
        public Mat Template { get; internal set; }
        public Mat Image { get; internal set; }

    }
}
