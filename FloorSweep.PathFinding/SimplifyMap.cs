using FloorSweep.Math;
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
            var @out = Mat.Zeros((int)(a / scaling), (int)(b / scaling)).ToMat();
            double c = @out.Rows;
            double d = @out.Cols;

            var s = scaling;
            a = System.Math.Floor((double)(scaling - 1) / 2);
            b = scaling - a;

            for (int x = 1; x <= c; x++)
            {
                for (int y = 1; y <= d; y++)
                {
                    var tmp = mapB.SumRange((int)(x * s - a), (int)(x * s + b), (int)(y * s - a), (int)(y * s + b));
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
