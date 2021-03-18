using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FloorSweep.PathFinding
{
    public static class MatExtensions
    {
        public static void _Set<T>(this Mat m, int row, int col, double value) where T : unmanaged => m[row, col] = value;


    }
}
