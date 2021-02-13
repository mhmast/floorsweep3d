using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class PlottedPath
    {
        public Image Image { get; internal set; }
        public Mat Path { get; internal set; }
    }
}
