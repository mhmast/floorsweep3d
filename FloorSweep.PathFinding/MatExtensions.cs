using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace FloorSweep.PathFinding
{
    public static class MatExtensions
    {
        public static ref T At<T>(this Mat m, Point p) where T : unmanaged
        {
            return ref m.At<T>(p.X, p.Y);
        }

        public static Mat Plus(this Mat m, Mat other)
        {
            if(m.Empty())
            {
                return other.Copy();
            }
            if(other.Empty())
            {
                return m.Copy();
            }

            if (other.Size() != m.Size() && !(other.Rows == 1 || other.Cols ==1) && !(m.Rows == 1 || m.Cols == 1) )
            {
                throw new ArgumentException();
            }

            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 0; i < Math.Max(m.Rows,other.Rows); i++)
            {
                for (int c = 0; c < Math.Max(m.Cols,other.Cols); c++)
                {
                    var valCol = m.Cols == 1 ? 1 : c;
                    double val = m.Rows == 1 ? m._<double>(0, valCol) : m._<double>(i, valCol);
                    double oval = other.Rows == 1 ? other._<double>(0, c) : other._<double>(i, c);
                    retVal._<double>(i, c) = val + oval; 
                }
            }

            return retVal;
        }

        public static void AddBottom(this Mat m, params double[][] other)
        {
            AddBottom(m, FromRows(other));
        }
        public static void AddBottom(this Mat m, Mat other)
        {
            var rows = m.Rows;
            m.Resize(rows + other.Rows);
            for (int i = 0; i < other.Rows; i++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    m.At<double>(i + rows, c) = other._<double>(i, c);
                }
            }
        }
        
        public static void AddColumn(this Mat m, params double[][] other)
        {
            AddColumn(m, FromRows(other));
        }
        public static void AddColumn(this Mat m, Mat other)
        {
            Cv2.HConcat(m, other, m);
        }
        public static ref T _<T>(this Mat m, int pos) where T : unmanaged
        {
            for (int row = 0; row < m.Rows; row++)
            {
                for (int col = 0; col < m.Cols; col++)
                {
                    if (row + col == pos)
                    {
                        return ref m.At<T>(row, col);
                    }
                }
            }
            throw new Exception();
        }
        
        
        public static int __(this Mat m, int pos) 
        {
            for (int row = 0; row < m.Rows; row++)
            {
                for (int col = 0; col < m.Cols; col++)
                {
                    if (row + col == pos)
                    {
                        return (int)m.At<double>(row, col);
                    }
                }
            }
            throw new Exception();
        }

        public static int __(this Mat m, int x, int y)
        {
            return (int)m.At<double>(x,y);
        }

        public static ref T _<T>(this Mat m, int row, int col) where T : unmanaged
        {
            if (m.Rows <= row )
            {
                m.Resize(row + 1);
            }
            if (m.Cols <= col )
            {
                Cv2.HConcat(m, Mat.Zeros(m.Type(), m.Rows, col + 1 - m.Cols), m);
            }
            return ref m.At<T>(row, col);
        }

        public static T[] DataLeftToRight<T>(this Mat m) where T : unmanaged
        {
            var res = new List<T>();
            for (int row = 0; row < m.Rows; row++)
            {
                for (int col = 0; col < m.Cols; col++)
                {
                    res.Add(m.At<T>(row, col));
                }
            }
            return res.ToArray();
        }

        public static void AddBottom(this Mat m, double other)
        {
            var data = m.DataLeftToRight<double>();
            m.Resize(m.Rows + 1);
            m.SetArray(data.Concat(new[] { other }).ToArray());
        }

        public static double Sum(this Scalar m)
        {
            return m.Val0 + m.Val1 + m.Val2 + m.Val3;
        }
        public static double Sum2(this Mat m)
        {
            return m.Sum().Sum();
        }

        public static Mat FromRange(double start, double end, double step)
        {
            var arr = new List<double>();
            for (var i = start; i <= end; i += step)
            {
                arr.Add(i);
            }
            return Mat.FromArray(arr.ToArray());
        }
        public static Mat FromRows(params double[][] rows)
        {
            var len = rows[0].Length;
            if (rows.All(r => r.Length != len))
            {
                throw new ArgumentException();
            }
            if (len == 0)
            {
                return new Mat(rows: rows.Length, len, MatType.CV_64FC1);
            }
            var arr = new double[rows.Length, len];
            for (int r = 0; r < rows.Length; r++)
            {
                var row = rows[r];
                for (int j = 0; j < len; j++)
                {
                    arr[r, j] = row[j];
                }
            }
            return Mat.FromArray(arr);
        }
        public static Mat ComplexConjugate(this Mat m)
        {
            Mat ones = Mat.Ones(m.Rows, m.Cols, m.Type()).ToMat();
            Mat ret = Mat.Zeros(m.Rows, m.Cols, m.Type()).ToMat();
            Cv2.MulSpectrums(ones, m, ret, DftFlags.None, true);
            return ret;
        }

        public static Mat RemoveRows(this Mat m, params int[] rows)
        {
            Mat ret = new Mat();
            for (int i = 0; i < m.Rows; i++)
            {
                if (!rows.Contains(i))
                {
                    ret.Add(m.Row(i));
                }
            }
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

        public static Mat Pow(this Mat m, double pow)
        {
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => *val = Math.Pow(*val, pow)));
            }
            return m;
        }
        
        public static Mat Copy(this Mat s)
        {
            return new Mat(s, new Rect(new Point(0, 0), s.Size()));
        }
        public static bool IsEqual(this Mat m, Mat other)
        {
            if(m.Size() != other.Size())
            {
                return false;
            }
            for (int row = 0; row < m.Rows; row++)
            {
                for (int col = 0; col < m.Cols; col++)
                {
                    var val = m._<double>(row, col);
                    var o = other._<double>(row,col);
                    if(val != o)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Mat UniqueRows(this Mat m)
        {
            var retVals = new List<List<double>>();

            for (int row = 0; row < m.Rows; row++)
            {
                var rowVals = new List<double>();
                for (int col = 0; col < m.Cols; col++)
                {
                    var val = m._<double>(row, col);
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
                    if (expr(m._<double>(row, column)))
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
                var mret = new Mat(m.Rows,1,m.Type());
                for(int r =0;r<m.Rows;r++)
                {
                    mret._<double>(r, i) = m._<double>(r, i);
                }
                yield return mret;
                
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

        public static Mat Round(this Mat m)
        {
            unsafe
            {
                m.ForEachAsDouble(new MatForeachFunctionDouble((val, pos) => *val = Math.Round(*val)));
            }
            return m;
        }

    }
}
