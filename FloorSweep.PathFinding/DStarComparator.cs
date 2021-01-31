using OpenCvSharp;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class DStarComparator : IComparer<Mat>
    {
        public int Compare(Mat m, Mat m2)
        {
            m.T().ToMat().GetArray(out double[] array);
            m2.T().ToMat().GetArray(out double[] array2);
            if (array[1] < array2[1] || (array[1] == array2[1] && array[2] < array2[2]))
            {
                return -1;
            }
            if (array[1] > array2[1] || (array[1] == array2[1] && array[2] > array2[2]))
            {
                return 1;
            }
            return 0;
        }
    }
}



