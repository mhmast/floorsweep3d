
using FloorSweep.Math;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    public class State
    {
        private Mat _map;
        private List<Point> _path = new List<Point>();

        public event Action PathFound;
        public Mat Map
        {
            get { return _map; }
            set
            {
                _map = value;
                Vis = Mat.Zeros(Height, Width);
                Template = Mat.Zeros(Height, Width);
            }
        }
        public Point StartPos { get; internal set; }
        public Point EndPos { get; internal set; }
        public int Scaling { get; internal set; }
        public IEnumerable<Point> Pattern { get; internal set; }
        public IEnumerable<Point> Ucc { get; internal set; }
        public int Height { get => Map.Rows; }
        public int Width { get => Map.Cols; }
        public Mat[] Graph { get; internal set; }
        public double KM { get; set; }
        public SortedSet<Point4> Stack { get; internal set; }
        public bool Exist { get; internal set; }
        public double Length { get; internal set; }
        public List<Point> Path
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
