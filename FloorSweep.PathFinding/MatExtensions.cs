using OpenCvSharp;
using System;
using System.Collections.Generic;

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
        
        public static IEnumerable<double> AsMathlabEnumerable(this Mat m)
        {
            m.T().ToMat().GetArray(out double[] data);
            return data;
        }

        public static double Min(this Mat m)
        {
            double min = double.MaxValue;
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => min = *val < min ? *val : min));
            }
            return min;
        }
        
        public static double Max(this Mat m)
        {
            double max = double.MinValue;
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => max = *val > max ? *val : max));
            }
            return max;
        }

        public static Mat SetAll(this Mat m, double d)
        {
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => *val = d));
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
