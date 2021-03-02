using OpenCvSharp;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class DStarComparator : IComparer<Mat>
    {
        public int Compare(Mat m, Mat m2)
        {
            var array = m.DataLeftToRight<double>();
            var array2 = m2.DataLeftToRight<double>();
            if (array[0] < array2[0] || (array[0] == array2[0] && array[1] < array2[1]))
            {
                return -1;
            }
            if (array[0] > array2[0] || (array[0] == array2[0] && array[1] > array2[1]))
            {
                return 1;
            }
            return 0;
        }
    }
}



