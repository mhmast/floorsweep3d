
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FloorSweep.Math
{
    public class Mat
    {
        private double[][] _data;

        public event Action<int, int, double> MatChanged;
        public Mat(int rows, int cols, double value)
        : this(rows, cols, GenerateData(rows, cols, value)) { }

        public OpenCvSharp.Mat ToCvMat()
         => new OpenCvSharp.Mat(rows: Rows, Cols, OpenCvSharp.MatType.CV_64FC1, _data);


        private static double[][] GenerateData(int rows, int cols, double value)
        {
            if (value == 0) return GenerateDataZero(rows, cols);
            var data = new double[rows][];
            for (var r = 0; r < rows; r++)
            {
                var c = new double[cols];
                Array.Fill(c, value);
                data[r] = c;
            }
            return data;
        }

        private static double[][] GenerateDataZero(int rows, int cols)
        {
            var data = new double[rows][];
            for (var r = 0; r < rows; r++)
            {
                data[r] = new double[cols];
            }
            return data;
        }



        public Mat(int rows = 0, int cols = 0, double[] data = null) : this(rows, cols, FormatData(data, rows, cols))
        {
        }

        private Mat(int rows = 0, int cols = 0, double[][] data = null)
        {

            if (data != null)
            {
                if (data.Length != rows || (cols > 0 && data[0].Length != cols))
                {
                    throw new ArgumentException();
                }
                _data = data;
            }
            else
            {
                _data = GenerateDataZero(rows, cols);
            }

            Rows = rows;
            Cols = cols;
        }

        private static double[][] FormatData(double[] data, int rows, int cols)
        {
            if (data == null) return null;
            var newData = new double[rows][];
            for (int r = 0; r < rows; r++)
            {
                newData[r] = new double[cols];
                Array.Copy(data, r * cols, newData[r], 0, cols);
            }
            return newData;
        }

        public static Mat Zeros(int rows, int cols)
        {
            return new Mat(rows, cols, GenerateDataZero(rows, cols));
        }

        public double SumAll() => Data.Sum();

        public Mat T()
        {
            var newData = new double[Cols][];
            for (int c = 0; c < Cols; c++)
            {
                newData[c] = _data.Select(r => r[c]).ToArray();
            }
            var retMat = new Mat(Cols, Rows, newData);
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
                return _data[row - 1][col - 1];
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
                // var addr = GetArrAddr(row, col);
                //var prev = _data[addr];
                _data[row - 1][col - 1] = value;
                //if (prev != value)
                //{
                //    
                //    MatChanged?.Invoke(row, col, value);
                //}
            }
        }

        public Mat Range(int startRow, int endRow, int startCol, int endCol)
        {
            if (startRow * endRow * startCol * endCol == 0)
            {
                throw new ArgumentException();
            }
            var noRows = endRow - startRow + 1;
            var noCols = endCol - startCol + 1;
            var overlapCols = System.Math.Min(Cols, endCol) - startCol+1;
            var overlapRows = System.Math.Min(Rows, endRow) - startRow+1;
            var retMat = Zeros(noRows, noCols);
            if (overlapCols == 0 || overlapRows == 0)
            {
                return retMat;
            }

            for (int row = 0; row < overlapRows; row++)
            {
                Array.Copy(_data[row+startRow-1], startCol-1, retMat._data[row], 0, overlapCols);
            }

            return retMat;

            //for (var row = startRow; row <= minRow; row++)
            //{
            //    var addr = GetArrAddr(row, startCol);
            //    Array.Copy(_data, addr, newData, (row - startRow) * noCols, minCol - startCol + 1);
            //    //for (var col = startCol; col <= minCol; col++)
            //    //{
            //    //    var addr2 = GetArrAddr(row, col);
            //    //    newData2[row*noCols+(col - startCol)] = _data[addr2];
            //    //}
            //}
        }

        public IEnumerable<Mat> Columns()
        {
            for (int c = 0; c < Cols; c++)
            {
                yield return new Mat(Rows, 1, _data.Select(r => r[c]).ToArray());
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

        public void SetAll(double val)
        {
            _data = GenerateData(Rows, Cols, val);
        }

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
            var newData = new double[Rows][];

            for (int i = 0; i < System.Math.Max(Rows, other.Rows); i++)
            {
                var newCols = new double[Cols];

                for (int c = 0; c < System.Math.Max(Cols, other.Cols); c++)
                {
                    var valCol = Cols == 1 ? 0 : c;
                    var val = Rows == 1 ? _data[0][valCol] : _data[i][valCol];
                    var ovalCol = other.Cols == 1 ? 0 : c;
                    var oval = other.Rows == 1 ? other._data[0][ovalCol] : other._data[i][ovalCol];
                    newCols[c] = val - oval;
                }
                newData[i] = newCols;
            }
            return new Mat(Rows, Cols, newData);

        }
        public Mat Minus(double value)
            => ForeachInThisMat(d => d - value);

        public static Mat operator -(Mat m, double value) => m.Minus(value);


        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public double[] Data => _data.SelectMany(s => s).ToArray();

        public double Max()
        => Data.Max();

        public double Min()
        => Data.Min();
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
                1 when m.Channels() == 1 => GetArray<byte>(m),
                8 when m.Channels() == 1 => GetArray<double>(m),
                _ => throw new ArgumentException()
            };

        private static double[] GetArray<T>(OpenCvSharp.Mat m) where T : unmanaged
        {
            m.GetArray<T>(out var data);
            return data.Select(t => Convert.ToDouble(t)).ToArray();
        }

        public Size Size()
        => new Size(Cols, Rows);

        public bool IsEqual(Mat other)
        {
            if (Size() != other.Size())
            {
                return false;
            }
            return Data.SequenceEqual(other.Data);
        }

        public Mat Pow(double power) => ForeachInThisMat(d => System.Math.Pow(d, power));
        public Mat Floor() => ForeachInThisMat(System.Math.Floor);
        public Mat Abs() => ForeachInThisMat(System.Math.Abs);
        private Mat ForeachInThisMat(Func<double, double> f) => new Mat(Rows, Cols, Data.Select(f).ToArray());

        internal void VConcat(Mat other)
        {
            var newRows = (Rows + other.Rows);
            var newData = new double[newRows][];
            if (other.Cols != Cols)
            {
                throw new ArgumentException();
            }
            ;
            Array.Copy(_data, newData, Rows);
            Array.Copy(other._data, 0, newData, Rows, other.Rows);
            _data = newData;
            Rows = newRows;
        }

        internal void HConcat(Mat other)
        {
            var newcols = (Cols + other.Cols);
            var newData = new double[Rows, newcols];
            if (other.Rows != Rows)
            {
                throw new ArgumentException();
            }
            for (int r = 0; r < Rows; r++)
            {
                _data[r] = _data[r].Concat(other._data[r]).ToArray();
            }
            Cols = newcols;
        }
    }
}
