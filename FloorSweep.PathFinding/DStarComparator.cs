using FloorSweep.Math;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class DStarComparator : IComparer<Point4>
    {
        public int Compare(Point4 m, Point4 m2)
        {
            //var mDist = System.Math.Abs(_target.Sum() - m.XY.Sum());
            //var m2Dist = System.Math.Abs(_target.Sum() - m2.XY.Sum());
            var cmp = m.Length.CompareTo(m2.Length);
            if (cmp == 0)
            {
                return m.XY.X.CompareTo(m2.XY.X);
            }
            return cmp;
        }
    }
}



