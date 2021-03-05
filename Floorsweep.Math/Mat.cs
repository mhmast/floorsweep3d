using FloorSweep.PathFinding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FloorSweep.Math
{
    public class Mat
    {
        private double[] _data;

        public event Action<int, int, double> MatChanged;
        public Mat(int rows, int cols, double value)
        : this(rows, cols, GenerateData(rows * cols, value)) { }

        public OpenCvSharp.Mat ToCvMat()
         => new OpenCvSharp.Mat(rows: Rows, Cols, OpenCvSharp.MatType.CV_64FC1, _data);



        private static double[] GenerateData(int size, double value)
        {
            var data = new double[size];
            Array.Fill(data, value);
            return data;
        }

        public Mat(int rows = 0, int cols = 0, double[] data = null)
        {
            if (data != null)
            {
                if (data.Length != rows * cols)
                {
                    throw new ArgumentException();
                }
                _data = data;
            }
            else
            {
                _data = new double[rows * cols];
            }

            Rows = rows;
            Cols = cols;
        }

        public static Mat Zeros(int rows, int cols)
        {
            var data = new double[rows * cols];
            Array.Fill(data, 0);
            return new Mat(rows, cols, data);
        }

        public double SumAll() => _data.Sum();

        public Mat T()
        {
            var data = new double[_data.Length];
            var retMat = new Mat(Cols, Rows, data);
            for (var i = 1; i <= _data.Length; i++)
            {
                var (row, col) = GetRowCol(i);
                retMat[col, row] = this[row, col];
            }
            return retMat;
        }

        public bool Empty()
        => _data.Length == 0;

        public double this[int row, int col]
        {
            get
            {
                //if (row == 0 || col == 0)
                //{
                //    throw new ArgumentException();
                //}
                //if (Rows < row)
                //{
                //    VConcat(Zeros(row - Rows, Cols));
                //}
                //if (Cols < col)
                //{
                //    HConcat(Zeros(Rows, col - Cols));
                //}
                return _data[GetArrAddr(row, col)];
            }
            set
            {
                //if (row == 0 || col == 0)
                //{
                //    throw new ArgumentException();
                //}
                //if (Rows < row)
                //{
                //    VConcat(Zeros(row - Rows, Cols));
                //}
                //if (Cols < col)
                //{
                //    HConcat(Zeros(Rows, col - Cols));
                //}
                var addr = GetArrAddr(row, col);
                //var prev = _data[addr];
                _data[addr] = value;
                //if (prev != value)
                //{
                //    
                //    MatChanged?.Invoke(row, col, value);
                //}
            }
        }

        public Mat Range(int startRow, int endRow, int startCol, int endCol)
        {
            var noRows = endRow - startRow + 1;
            var noCols = endCol - startCol + 1;
            var newData = new double[noRows * noCols];
            var minCol = System.Math.Min(Cols, endCol);
            var minRow = System.Math.Min(Rows, endRow);
            for (var row = startRow; row <= minRow; row++)
            {
                var addr = GetArrAddr(row, startCol);
                Array.Copy(_data, addr, newData, (row - startRow) * noCols, minCol - startCol+1);
                //for (var col = startCol; col <= minCol; col++)
                //{
                //    var addr2 = GetArrAddr(row, col);
                //    newData2[row*noCols+(col - startCol)] = _data[addr2];
                //}
            }
            return new Mat(noRows, noCols, newData);
        }

        public IEnumerable<Mat> Columns()
        {
            for (int c = 1; c <= Cols; c++)
            {
                yield return new Mat(Rows, 1, Enumerable.Range(1, Rows).Select(r => _data[GetArrAddr(r, c)]).ToArray());
            }
        }

        public Mat ToMat() => this;
        public double this[int pos]
        {
            get
            {

                var (row, col) = GetRowCol(pos);
                return this[row, col];
            }
            set
            {
                var (row, col) = GetRowCol(pos);
                this[row, col] = value;
            }
        }

        public void SetAll(double val) => Array.Fill(_data, val);

        public Mat Copy()
        => new Mat(Rows, Cols, _data);
        public Mat Div(double value)
            => ForeachInThisMat(d => d / value);

        public static Mat operator /(Mat m, double value) => m.Div(value);

        public Mat Plus(double value)
            => ForeachInThisMat(d => d + value);

        public static Mat operator +(Mat m, double value) => m.Plus(value);

        public Mat Mul(double value)
            => ForeachInThisMat(d => d * value);

        public static Mat operator *(Mat m, double value) => m.Mul(value);


        public Mat Minus(Mat other)
        {
            if (Empty())
            {
                return other.Copy();
            }
            if (other.Empty())
            {
                return Copy();
            }

            if (other.Size() != Size() && !(other.Rows == 1 || other.Cols == 1) && !(Rows == 1 || Cols == 1))
            {
                throw new ArgumentException();
            }

            Mat retVal = new Mat(Rows, Cols);

            for (int i = 1; i <= System.Math.Max(Rows, other.Rows); i++)
            {
                for (int c = 1; c <= System.Math.Max(Cols, other.Cols); c++)
                {
                    var valCol = Cols == 1 ? 1 : c;
                    var val = Rows == 1 ? this[1, valCol] : this[i, valCol];
                    var ovalCol = other.Cols == 1 ? 1 : c;
                    var oval = other.Rows == 1 ? other[1, ovalCol] : other[i, ovalCol];
                    retVal[i, c] = val - oval;
                }
            }
            return retVal;
        }
        public Mat Minus(double value)
            => ForeachInThisMat(d => d - value);

        public static Mat operator -(Mat m, double value) => m.Minus(value);


        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public double[] Data => _data;

        public double Max()
        => _data.Max();

        public double Min()
        => _data.Min();

        private int GetArrAddr(int row, int col)
        => ((row - 1) * Cols) + (col - 1);

        private (int, int) GetRowCol(int pos)
        {
            var row = (pos / Cols) + (pos % Cols > 0 ? 1 : 0);
            return (row, pos - ((row - 1) * Cols));
        }

        public static Mat FromCV(OpenCvSharp.Mat cvMat)
        {
            return new Mat(cvMat.Rows, cvMat.Cols, ResolveCvData(cvMat));
        }

        public static double[] ResolveCvData(OpenCvSharp.Mat m)
            => m.ElemSize() switch
            {
                1 when m.Channels() == 1 => m.DataLeftToRight<byte>().Select(b => (double)b).ToArray(),
                8 when m.Channels() == 1 => m.DataLeftToRight<double>(),
                _ => throw new ArgumentException()
            };



        public Size Size()
        => new Size(Cols, Rows);

        public bool IsEqual(Mat other)
        {
            if (Size() != other.Size())
            {
                return false;
            }
            return _data.SequenceEqual(other._data);
        }

        public Mat Pow(double power) => ForeachInThisMat(d => System.Math.Pow(d, power));
        public Mat Floor() => ForeachInThisMat(System.Math.Floor);
        public Mat Abs() => ForeachInThisMat(System.Math.Abs);
        private Mat ForeachInThisMat(Func<double, double> f) => new Mat(Rows, Cols, _data.Select(f).ToArray());

        internal void VConcat(Mat other)
        {
            var newRows = (Rows + other.Rows);
            var newData = new double[newRows * Cols];
            if (other.Cols != Cols)
            {
                throw new ArgumentException();
            }
            ;
            Array.Copy(_data, newData, _data.Length);
            Array.Copy(other._data, 0, newData, _data.Length, other._data.Length);
            _data = newData;
            Rows = newRows;
        }

        internal void HConcat(Mat other)
        {
            var newcols = (Cols + other.Cols);
            var newData = new double[Rows * newcols];
            if (other.Rows != Rows)
            {
                throw new ArgumentException();
            }
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    newData[r * newcols + c] = _data[r * Cols + c];
                }
                for (int c = 0; c < other.Cols; c++)
                {
                    newData[r * newcols + c] = other._data[r * other.Cols + c];
                }
            }
            _data = newData;
            Cols = newcols;
        }
    }
}
