using FloorSweep.Math;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class DStarComparator : IComparer<Point4>
    {
        private readonly Point _target;

        public DStarComparator(Point target)
        {
            _target = target;
        }
        public int Compare(Point4 m, Point4 m2)
        {
            var mDist = System.Math.Abs(_target.Sum() - m.XY.Sum());
            var m2Dist = System.Math.Abs(_target.Sum() - m2.XY.Sum());
            var cmp = mDist.CompareTo(m2Dist);
            if (cmp == 0)
            {
                return m.XY.X == m2.XY.X ? 0 : -1;
            }
            return cmp;
        }
    }
}



