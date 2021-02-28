using OpenCvSharp;
using System;

namespace FloorSweep.PathFinding
{
    internal class SimplifyMap
    {
        public static Mat DoSimplifyMap(Mat map, int scaling)
        {

            var mapB = map;
            double a = mapB.Rows;
            double b = mapB.Cols;
            var @out = Mat.Zeros((int)(a / scaling), (int)(b / scaling), MatType.CV_64FC1).ToMat();
            double c = @out.Rows;
            double d = @out.Cols;

            var s = scaling;
            a = Math.Floor((double)(scaling - 1) / 2);
            b = scaling - a;

            for (int x = 0; x < c - 1; x++)
            {
                for (int y = 0; y < d - 1; y++)
                {
                    var tmp = mapB.Range((int)(x * s - a + 1), (int)(x * s + b + 1), (int)(y * s - a + 1), (int)(y * s + b + 1)).Sum2();

                    if (tmp < 12)
                    {
                        @out._Set<double>(x, y, 0);
                    }
                    else
                    {
                        @out._Set<double>(x, y, 1);
                    }
                }
            }
            return @out;
        }
    }
}
