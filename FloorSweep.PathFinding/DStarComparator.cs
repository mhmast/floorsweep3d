using OpenCvSharp;
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
            var mDist = Math.Abs(_target.Rows(1, 2).Minus(m.Rows(1, 2)).Sum2());
            var m2Dist = Math.Abs(_target.Rows(1, 2).Minus(m2.Rows(1, 2)).Sum2());
            var cmp = mDist.CompareTo(m2Dist);
            if (cmp == 0 && m.IsEqual(m2))
            {
                return 0;
            }
            return cmp == 0 ? -1 : cmp;
        }
    }
}



