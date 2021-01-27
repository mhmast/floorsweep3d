using OpenCvSharp;
using System;

namespace FloorSweep.PathFinding
{
    public static class MatExtensions
    {
        public static Mat ComplexConjugate(this Mat m)
        {
            Mat ones = Mat.Ones(rows: m.Width, m.Height, m.Type()).ToMat();
            Mat ret = Mat.Zeros(rows: m.Width, m.Height, m.Type()).ToMat();
            Cv2.MulSpectrums(ones, m, ret, DftFlags.None, true);
            return ret;
        }
        public static Mat Floor(this Mat m)
        {
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => *val = Math.Floor(*val)));
            }
            return m;
        }
        
        public static Mat Ceil(this Mat m)
        {
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => *val = Math.Ceiling(*val)));
            }
            return m;
        }
    }
}
