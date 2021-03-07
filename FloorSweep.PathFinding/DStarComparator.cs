using FloorSweep.Math;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class DStarComparator : IComparer<Mat>
    {
        private readonly Mat _target;

        public DStarComparator(Mat target)
        {
            _target = target;
        }
        public int Compare(Mat m, Mat m2)
        {
            var mDist = System.Math.Abs(_target[1, 1] - m[1, 1] + _target[2, 1] - m[2, 1]);
            var m2Dist = System.Math.Abs(_target[1, 1] - m2[1, 1] + _target[2, 1] - m2[2, 1]);
            var cmp = mDist.CompareTo(m2Dist);
            if (cmp == 0 && m[1, 1] == m2[1, 1])
            {
                return 0;
            }
            return cmp == 0 ? -1 : cmp;
        }
    }
}



