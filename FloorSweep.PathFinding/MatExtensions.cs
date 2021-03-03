using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FloorSweep.PathFinding
{
    public static class MatExtensions
    {


        private static event Action<Mat, int, int, double> MatChangedd;
        private static event Action<Mat, int, int, byte> MatChangedb;
        public static void RegisterMatChanged(this Mat m, Action<int, int, double> action)
        {
            MatChangedd += (mat, x, y, n) =>
            {
                if (m == mat)
                {
                    action(x, y, n);
                }
            };
        }

        public static Mat _T(this Mat m)
        {
            if (m.Cols == 0 || m.Rows == 0)
            {
                return new Mat(m.Cols, m.Rows, m.Type());
            }
            return m.T().ToMat();
        }

        public static void SetColRange(this Mat m, int startcol, int endCol, double value)
        {
            if (m.Cols < endCol)
            {
                Cv2.HConcat(m, new Mat(rows: m.Rows, endCol - m.Cols + 1, m.Type()), m);
            }
            for (var r = 0; r < m.Rows; r++)
            {
                for (int c = startcol; c <= endCol; c++)
                {
                    m._Set(r, c, value);
                }
            }
        }
        public static void RegisterMatChanged(this Mat m, Action<int, int, byte> action)
        {
            MatChangedb += (mat, x, y, n) =>
            {
                if (m == mat)
                {
                    action(x, y, n);
                }
            };
        }

        public static ref T At<T>(this Mat m, Point p) where T : unmanaged
        {
            return ref m.At<T>(p.X, p.Y);
        }

        public static Mat Plus(this Mat m, Mat other)
        {
            if (m.Empty())
            {
                return other.Copy();
            }
            if (other.Empty())
            {
                return m.Copy();
            }

            if (other.Size() != m.Size() && !(other.Rows == 1 || other.Cols == 1) && !(m.Rows == 1 || m.Cols == 1))
            {
                throw new ArgumentException();
            }

            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 1; i <= Math.Max(m.Rows, other.Rows); i++)
            {
                for (int c = 1; c <= Math.Max(m.Cols, other.Cols); c++)
                {
                    var valCol = m.Cols == 1 ? 1 : c;
                    var val = m.Rows == 1 ? m._<double>(1, valCol) : m._<double>(i, valCol);
                    var ovalCol = other.Cols == 1 ? 1 : c;
                    var oval = other.Rows == 1 ? other._<double>(1, ovalCol) : other._<double>(i, ovalCol);
                    retVal.___<double>(i, c) = val + oval;
                }
            }

            return retVal;
        }

        public static Mat Minus(this Mat m, Mat other)
        {
            if (m.Empty())
            {
                return other.Copy();
            }
            if (other.Empty())
            {
                return m.Copy();
            }

            if (other.Size() != m.Size() && !(other.Rows == 1 || other.Cols == 1) && !(m.Rows == 1 || m.Cols == 1))
            {
                throw new ArgumentException();
            }

            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 1; i <= Math.Max(m.Rows, other.Rows); i++)
            {
                for (int c = 1; c <= Math.Max(m.Cols, other.Cols); c++)
                {
                    var valCol = m.Cols == 1 ? 1 : c;
                    var val = m.Rows == 1 ? m._<double>(1, valCol) : m._<double>(i, valCol);
                    var ovalCol = other.Cols == 1 ? 1 : c;
                    var oval = other.Rows == 1 ? other._<double>(1, ovalCol) : other._<double>(i, ovalCol);
                    retVal.___<double>(i, c) = val - oval;
                }
            }

            return retVal;
        }
        public static Mat Minus(this Mat m, double other) 
        {
            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 1; i <= m.Rows; i++)
            {
                for (int c = 1; c <= m.Cols; c++)
                {
                    retVal.___<double>(i, c) = m._<double>(i,c) - other;
                }
            }

            return retVal;
        }
        
        public static Mat Mult(this Mat m, double other) 
        {
            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 1; i <= m.Rows; i++)
            {
                for (int c = 1; c <= m.Cols; c++)
                {
                    retVal.___<double>(i, c) = m._<double>(i,c) * other;
                }
            }

            return retVal;
        }
        
        public static Mat Div(this Mat m, double other) 
        {
            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 1; i <= m.Rows; i++)
            {
                for (int c = 1; c <= m.Cols; c++)
                {
                    retVal.___<double>(i, c) = m._<double>(i,c) / other;
                }
            }

            return retVal;
        }
        
        public static Mat Plus(this Mat m, double other) 
        {
            Mat retVal = new Mat(m.Rows, m.Cols, m.Type());

            for (int i = 1; i <= m.Rows; i++)
            {
                for (int c = 1; c <= m.Cols; c++)
                {
                    retVal.___<double>(i, c) = m._<double>(i,c) + other;
                }
            }

            return retVal;
        }

        public static Mat Rows(this Mat m, int startRow, int endRow)
        => m.Range(startRow, endRow, 1, m.Cols);

        public static Mat Range(this Mat m, int startRow, int endRow, int startCol, int endCol)
        {
            Mat retVal = new Mat(endRow - startRow, endCol - startCol, m.Type());

            for (int i = startRow; i <= endRow; i++)
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    retVal.___<double>(i - startRow + 1, c - startCol + 1) = m._<double>(i, c);
                }
            }
            return retVal;
        }



        public static Mat Cols(this Mat m, int startCol, int endCol)
        => m.Range(1, m.Rows, startCol, endCol);

        public static void AddBottom(this Mat m, params double[][] other)
        {
            AddBottom(m, FromRows(other));
        }
        public static void AddBottom(this Mat m, Mat other)
        {
            if (m.Cols != other.Cols)
            {
                throw new ArgumentException();
            }
            var rows = m.Rows;
            m.Resize(rows + other.Rows);
            for (int i = 1; i <= other.Rows; i++)
            {
                for (int c = 1; c <= m.Cols; c++)
                {
                    m._Set<double>(i + rows, c, other._<double>(i, c));
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
        public static T _<T>(this Mat m, int pos) where T : unmanaged
        => m.___<T>(pos);
        private static ref T ___<T>(this Mat m, int pos) where T : unmanaged
        {
            for (int row = 0; row < m.Rows; row++)
            {
                for (int col = 0; col < m.Cols; col++)
                {
                    if (row + col +1 == pos)
                    {
                        return ref m.At<T>(row , col );
                    }
                }
            }
            throw new Exception();
        }


        public static int __(this Mat m, int pos)
        {
            for (int col = 0; col <= m.Cols; col++)
            {
                for (int row = 0; row <= m.Rows; row++)
                {

                    if (row + col + 1 == pos)
                    {
                        return (int)m.___<double>(row + 1, col + 1);
                    }
                }
            }
            throw new Exception();
        }

        public static int __(this Mat m, int x, int y)
        {
            return (int)m.___<double>(x, y);
        }

        public static T _<T>(this Mat m, int row, int col) where T : unmanaged
            => m.___<T>(row, col);
        private static ref T ___<T>(this Mat m, int row, int col) where T : unmanaged
        {
            if (row == 0 || col == 0)
            {
                throw new ArgumentException();
            }
            if (m.Rows < row)
            {
                m.Resize(row);
            }
            if (m.Cols < col)
            {
                Cv2.HConcat(m, Mat.Zeros(m.Type(), m.Rows, col - m.Cols), m);
            }

            return ref m.At<T>(row - 1, col - 1);
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
            return new Mat(rows: 1, arr.Count, MatType.CV_64FC1, arr.ToArray());
        }

        public static Mat FromCols(params double[][] cols)
        {

            var len = cols[0].Length;
            if (cols.All(r => r.Length != len))
            {
                throw new ArgumentException();
            }
            if (len == 0)
            {
                return new Mat(rows: len, cols.Length, MatType.CV_64FC1);
            }
            var arr = new double[cols.Length, len];
            for (int r = 0; r < cols.Length; r++)
            {
                var col = cols[r];
                for (int j = 0; j < len; j++)
                {
                    arr[j, r] = col[r];
                }
            }
            return Mat.FromArray(arr);
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


        public static Mat RemoveRows(this Mat m, params int[] rows)
        {
            Mat ret = new Mat(rows:0,m.Cols,m.Type());
            for (int i = 1; i <= m.Rows; i++)
            {
                if (!rows.Contains(i))
                {
                    ret.AddBottom(m.Rows(i,i));
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
            if (m.Size() != other.Size())
            {
                return false;
            }
            for (int row = 1; row <= m.Rows; row++)
            {
                for (int col = 1; col <= m.Cols; col++)
                {
                    var val = m._<double>(row, col);
                    var o = other._<double>(row, col);
                    if (val != o)
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

            for (int row = 1; row <= m.Rows; row++)
            {
                var rowVals = new List<double>();
                for (int col = 1; col <= m.Cols; col++)
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
            if (retVals.Count == 0)
            {
                return new Mat(rows: 0, 0, m.Type());
            }
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
            for (int row = 1; row <= m.Rows; row++)
            {
                for (int column = 1; column <= m.Cols; column++)
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
            for (int i = 1; i <= m.Cols; i++)
            {
                var mret = new Mat(m.Rows, 1, m.Type());
                for (int r = 1; r <= m.Rows; r++)
                {
                    mret.___<double>(r, 1) = m._<double>(r, i);
                }
                yield return mret;
            }
        }

        public static Mat Invert(this Mat m)
        {
            var ret = new Mat(m.Rows, m.Cols, m.Type());
            for(int row=0;row<m.Rows;row++)
            {
                for(int col=0;col<m.Cols;col++)
                {
                    ret.___<double>(m.Rows - row, m.Cols - col) = m.___<double>(row + 1, col + 1);
                }
            }
            return ret;
        }

        public static void _Set<T>(this Mat m, int row, int col, T value) where T : unmanaged
        {
            if (row == 0 || col == 0)
            {
                throw new ArgumentException();
            }

            if (typeof(T) == typeof(double))
            {
                MatChangedd?.Invoke(m, row, col, (double)Convert.ChangeType(value,typeof(T)));
            }
            else if (typeof(T) == typeof(byte))
            {
                MatChangedb?.Invoke(m, row, col, (byte)Convert.ChangeType(value,typeof(byte)));
            }
            m.___<T>(row, col) = value;
        }

        public static void _Set<T>(this Mat m, int pos, T value) where T : unmanaged
        {
            for (int row = 1; row <= m.Rows; row++)
            {
                for (int col = 1; col <= m.Cols; col++)
                {
                    if (row + col == pos)
                    {
                        if (typeof(T) == typeof(double))
                        {
                            MatChangedd?.Invoke(m, row, col, m.___<double>(row, col));
                        }
                        else if (typeof(T) == typeof(byte))
                        {
                            MatChangedb?.Invoke(m, row, col, m.___<byte>(row, col));
                        }
                        m.___<T>(row, col) = value;
                    }
                }
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
