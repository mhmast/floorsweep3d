using OpenCvSharp;

namespace FloorSweep.PathFinding
{
    internal class SimplifyMap
    {
        public static Mat DoSimplifyMap(Mat map, int scaling)
        {

            var mapB = map;
            var mapT = mapB.T().ToMat();
            var a = mapB.Rows;
            var b = mapB.Cols;
            var @out = Mat.Zeros(a / scaling, b / scaling, map.Type()).ToMat();
            var c = @out.Rows;
            var d = @out.Cols;

            var s = scaling;
            a = (scaling - 1) / 2;
            b = scaling - a;

            for (int x = 0; x < c - 1; x++)
            {
                for (int y = 0; y < d - 1; y++)
                {
                    var tmp = mapT.RowRange(x * s - a+1, x * s + b +1).ColRange(y * s - a+1, y * s + b+1).Sum2();
                    
                    if (tmp < 12)
                    {
                        @out.Set(x, y, 0);
                    }
                    else
                    {
                        @out.Set(x, y, 1);
                    }
                }
            }
            return @out;
        }
    }
}
