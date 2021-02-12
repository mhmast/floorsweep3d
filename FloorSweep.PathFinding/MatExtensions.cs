using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FloorSweep.PathFinding
{
    public static class MatExtensions
    {

        public static Mat FromRows(params double[][] rows)
        {
            var len = rows[0].Length;
            if (rows.All(r => r.Length != len))
            {
                throw new ArgumentException();
            }
            var arr = new double[rows.Length, len];
            for (int r = 0; r < rows.Length; r++)
            {
                arr.SetValue(rows[r], r);
            }
            return Mat.FromArray(arr);
        }
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

        public static Mat UniqueRows(this Mat m)
        {
            var retVals = new List<List<double>>();

            for (int row = 0; row < m.Rows; row++)
            {
                var rowVals = new List<double>();
                for (int col = 0; col < m.Cols; col++)
                {
                    var val = m.Get<double>(row, col);
                    rowVals.Add(val);
                }
                if (!retVals.Any(r => r.SequenceEqual(rowVals)))
                {
                    retVals.Add(rowVals);
                }
            }
            retVals.Sort(CompareDoubles);
            return FromRows(retVals.Select(r => r.ToArray()).ToArray());
        }

        private static int CompareDoubles(List<double> x, List<double> y)
        {
            if (x.Count != y.Count)
            {
                throw new ArgumentException();
            }
            for (int i = 0; i < x.Count; i++)
            {
                var cmp = x[i].CompareTo(y[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
            }
            return 0;
        }

        public static (Mat, Mat) Find(this Mat m, Predicate<double> expr)
        {
            var x = new Mat();
            var y = new Mat();
            for (int row = 0; row < m.Rows; row++)
            {
                for (int column = 0; column < m.Cols; column++)
                {
                    if (expr(m.Get<double>(row, column)))
                    {
                        x.Add(row);
                        y.Add(column);
                    }
                }
            }
            return (x, y);
        }

        public static IEnumerable<double> AsMathlabEnumerable(this Mat m)
        {
            m.T().ToMat().GetArray(out double[] data);
            return data;
        }

        public static IEnumerable<Mat> AsMathlabColEnumerable(this Mat m)
        {
            for (int i = 0; i < m.Cols; i++)
            {
                yield return m.ColRange(i, i);
            }
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
        => m.SetAll(_ => d);
        public static Mat SetAll(this Mat m, Func<double, double> df)
        {
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => *val = df(*val)));
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
