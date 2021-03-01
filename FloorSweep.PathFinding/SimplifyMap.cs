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

            for (int x = 1; x <= c; x++)
            {
                for (int y = 1; y <= d; y++)
                {
                    var tmp = mapB.Range((int)(x * s - a ), (int)(x * s + b ), (int)(y * s - a ), (int)(y * s + b )).Sum2();

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
