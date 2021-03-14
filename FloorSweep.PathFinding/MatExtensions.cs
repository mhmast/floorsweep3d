using FloorSweep.Math;
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
            //MatChangedd += (mat, x, y, n) =>
            //{
            //    if (m == mat)
            //    {
            //        action(x, y, n);
            //    }
            //};
#if DEBUG
            m.MatChanged += action;
#endif
        }


        public static Mat Rows(this Mat m, int startRow, int endRow)
        => m.Range(startRow, endRow, 1, m.Cols);

        public static void AddBottom(this Mat m, Mat other) => m.VConcat(other);


        public static void AddColumn(this Mat m, Mat other)
        => m.HConcat(other);

        public static int __(this Mat m, int pos)
        {
            for (int col = 0; col <= m.Cols; col++)
            {
                for (int row = 0; row <= m.Rows; row++)
                {

                    if (row + col + 1 == pos)
                    {
                        return (int)m[row + 1, col + 1];
                    }
                }
            }
            throw new Exception();
        }

        public static int __(this Mat m, int x, int y)
        {
            return (int)m.___<double>(x, y);
        }

        public static double _<T>(this Mat m, int row, int col) where T : unmanaged
            => m[row, col];

        private static double ___<T>(this Mat m, int row, int col) where T : unmanaged
        => m[row, col];
        public static Mat FromRows(params double[][] rows)
        {
            var len = rows[0].Length;
            if (rows.All(r => r.Length != len))
            {
                throw new ArgumentException();
            }
            if (len == 0)
            {
                return new Mat(rows.Length, len);
            }

            return new Mat(rows.Length, len, rows.SelectMany(r => r).ToArray());
        }





        public static void _Set<T>(this Mat m, int row, int col, double value) where T : unmanaged
        {
            if (row == 0 || col == 0)
            {
                throw new ArgumentException();
            }

            if (typeof(T) == typeof(double))
            {
                MatChangedd?.Invoke(m, row, col, (double)Convert.ChangeType(value, typeof(T)));
            }
            else if (typeof(T) == typeof(byte))
            {
                MatChangedb?.Invoke(m, row, col, (byte)Convert.ChangeType(value, typeof(byte)));
            }
            m[row, col] = value;
        }


    }
}
