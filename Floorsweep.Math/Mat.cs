
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

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

        public double SumAll()
        {
            var sum = 0.0;
            for (var row = 1; row < Rows; row++)
            {
                for (var col = 1; col < Cols; col++)
                {
                    sum += this[row, col];
                }
            }
            return sum;
        }
        public double Min()
        {
            var min = double.PositiveInfinity;
            for (var row = 1; row < Rows; row++)
            {
                for (var col = 1; col < Cols; col++)
                {
                    min = System.Math.Min(min, this[row, col]);
                }
            }
            return min;
        }

        public double Max()
        {
            var max = double.NegativeInfinity;
            for (var row = 1; row < Rows; row++)
            {
                for (var col = 1; col < Cols; col++)
                {
                    max = System.Math.Max(max, this[row, col]);
                }
            }
            return max;
        }

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
            var overlapCols = System.Math.Min(Cols, endCol) - startCol + 1;
            var overlapRows = System.Math.Min(Rows, endRow) - startRow + 1;
            var retMat = Zeros(noRows, noCols);
            if (overlapCols == 0 || overlapRows == 0)
            {
                return retMat;
            }

            for (int row = 0; row < overlapRows; row++)
            {
                Array.Copy(_data[row + startRow - 1], startCol - 1, retMat._data[row], 0, overlapCols);
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

        private Mat ProductTemplate(Mat other, Func<double, double, double> @operator)
        {
            if (Empty())
            {
                return other.Copy();
            }
            if (other.Empty())
            {
                return Copy();
            }

            if (other.Size() == Size())
            {
                return ProductTemplate(this, (row, col) => other[row, col], @operator);
            }
            if (Cols == other.Cols)
            {
                if (Rows == 1)
                {
                    return ProductTemplate(other, (row, col) => this[1, col], @operator);
                }
                if (other.Rows == 1)
                {
                    return ProductTemplate(this, (row, col) => other[1, col], @operator);
                }
            }
            else if (Rows == other.Rows)
            {
                if (Cols == 1)
                {
                    return ProductTemplate(other, (row, col) => this[row, 1], @operator);
                }
                if (other.Rows == 1)
                {
                    return ProductTemplate(this, (row, col) => other[row, 1], @operator);
                }
            }
            throw new ArgumentException();
        }

        public Point Point(int row, int col) => new Point((int)this[row, col], (int)this[row + 1, col]);

        private Mat ProductTemplate(Mat m, Func<int, int, double> valueSelector, Func<double, double, double> @operator)
        {
            var retVal = new double[m.Rows][];

            for (int i = 1; i <= m.Rows; i++)
            {
                var col = new double[m.Cols];
                for (int c = 1; c <= Cols; c++)
                {
                    col[c - 1] = @operator(m[i, c], valueSelector(i, c));
                }
                retVal[i - 1] = col;
            }
            return new Mat(Rows, Cols, retVal);
        }

        public Mat Mul(double value)
            => ProductTemplate(this, (row, col) => value, (l, r) => l * r);

        public Mat Mul(Mat value)
            => ProductTemplate(value, (l, r) => l * r);

        public static Mat operator *(Mat m, double value) => m.Mul(value);
        public static Mat operator *(Mat m, Mat value) => m.Mul(value);

        public Mat Minus(double value)
            => ProductTemplate(this, (row, col) => value, (l, r) => l - r);
        public Mat Minus(Mat value)
            => ProductTemplate(value, (l, r) => l - r);

        public static Mat operator -(Mat m, double value) => m.Minus(value);
        public static Mat operator -(Mat m, Mat value) => m.Minus(value);

        public Mat Div(double value)
            => ProductTemplate(this, (row, col) => value, (l, r) => l / r);
        public Mat Div(Mat value)
            => ProductTemplate(value, (l, r) => l / r);

        public static Mat operator /(Mat m, double value) => m.Div(value);
        public static Mat operator /(Mat m, Mat value) => m.Div(value);


        public Mat Plus(double value)
            => ProductTemplate(this, (row, col) => value, (l, r) => l + r);
        public Mat Plus(Mat value)
            => ProductTemplate(value, (l, r) => l + r);

        public static Mat operator +(Mat m, double value) => m.Plus(value);
        public static Mat operator +(Mat m, Mat value) => m.Plus(value);


        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public unsafe double[] Data
        {
            get
            {
                var dta = new double[Rows * Cols];
                for (var row = 0; row < Rows; row++)
                {
                    Array.Copy(_data[row], 0, dta, row * Cols, Cols);
                }
                return dta;
            }
        }

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
                1 when m.Channels() == 1 => GetByteArray(m),
                8 when m.Channels() == 1 => GetArray<double>(m),
                _ => throw new ArgumentException()
            };

        private unsafe static double[] GetByteArray(OpenCvSharp.Mat m)
        {
            var bytes = GetArray<byte>(m);
            return bytes.Select(Convert.ToDouble).ToArray();
        }
        private unsafe static double[] GetDoubleArray(OpenCvSharp.Mat m)
        => GetArray<double>(m);
        private unsafe static T[] GetArray<T>(OpenCvSharp.Mat m) where T : unmanaged
        {
            var buffer = new T[m.Rows * m.Cols];
            fixed (void* dtaPtr = &buffer[0])
            {
                var len = buffer.Length * sizeof(T);
                Buffer.MemoryCopy(m.DataPointer, dtaPtr, len, len);
                return buffer;
            }
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

        public Mat Pow(double power) => ProductTemplate(this, (r, c) => power, System.Math.Pow);
        public Mat Floor() => ProductTemplate(this, (r, c) => 0.0, (l, r) => System.Math.Floor(l));
        public Mat Abs() => ProductTemplate(this, (r, c) => 0.0, (l, r) => System.Math.Abs(l));

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
