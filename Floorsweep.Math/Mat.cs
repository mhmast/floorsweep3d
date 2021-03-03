using FloorSweep.PathFinding;
using System;
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
                if (row == 0 || col == 0)
                {
                    throw new ArgumentException();
                }
                if (Rows < row)
                {
                    VConcat(Zeros(row - Rows, Cols));
                }
                if (Cols < col)
                {
                    HConcat(Zeros(Rows, col - Cols));
                }
                return _data[GetArrAddr(row, col)];
            }
            set
            {
                if (row == 0 || col == 0)
                {
                    throw new ArgumentException();
                }
                if (Rows < row)
                {
                    VConcat(Zeros(row - Rows, Cols));
                }
                if (Cols < col)
                {
                    HConcat(Zeros(Rows, col - Cols));
                }
                var addr = GetArrAddr(row, col);
                var prev = _data[addr];
                if (prev != value)
                {
                    _data[addr] = value;
                    MatChanged?.Invoke(row, col, value);
                }
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
            return (row, pos - ((row-1) * Cols));
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
