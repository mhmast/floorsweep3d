using MathNet.Numerics.LinearAlgebra;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class MapData
    {
        public Mat Map { get; set; }
        public Mat Start { get; set; }
        public Mat Target { get; set; }
    }
}
