﻿using static System.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FloorSweep.Math
{
    public class Mat
    {
        private double[][] _data;
#if DEBUG
        public event Action<int, int, double> MatChanged;
#endif
        public Mat(int rows, int cols, double value)
        : this(rows, cols, GenerateData(rows, cols, value)) { }

        public static Mat Rotate(int degrees)
        {
            var radian = degrees * 0.0174532925;
            return new Mat(2, 2, new[] { new[] { Cos(radian), -Sin(radian) }, new[] { Sin(radian), Cos(radian) } });
        }

        public PointD Mul(PointD left)
        {
            if (Cols != 2 || Rows != 2)
            {
                throw new ArgumentException("Cannot multiply a non 2x2 Mat with a Point");
            }
            return new PointD(this[1, 1] * left.X + this[1, 2] * left.Y, (this[2, 1] * left.X) + (this[2, 2] * left.Y));
        }

        public Point Mul(Point left)
        {
            if (Cols != 2 || Rows != 2)
            {
                throw new ArgumentException("Cannot multiply a non 2x2 Mat with a Point");
            }
            return new Point((int)(this[1, 1] * left.X + this[1, 2] * left.Y), (int)((this[2, 1] * left.X) + (this[2, 2] * left.Y)));
        }

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

        public static Mat Ones(int rows, int cols)
        => new Mat(rows, cols, 1);

        public unsafe static Mat ImageToBinary(Bitmap b, double tresh = 126) => ImageTo(b, (b, g, r) => GrayScale(b, g, r) <= tresh ? 0 : 1);
        public unsafe static Mat ImageToGrayScale(Bitmap b) => ImageTo(b, GrayScale);

        private static double GrayScale(byte r, byte g, byte b)
            => (b * .11) + (g * .59) + (r * .3);


        private static unsafe Mat ImageTo(Bitmap b, Func<byte, byte, byte, double> valueSelector)
        {

            var newData = new double[b.Height][];
            var bits = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var pixelSize = bits.Stride / b.Width;
            if (pixelSize != 3)
            {
                throw new ArgumentException();
            }
            for (int y = 0; y < b.Height; y++)
            {
                //get the data from the original image
                byte* oRow = (byte*)bits.Scan0 + (y * bits.Stride);
                var row = new double[b.Width];

                for (int x = 0; x < b.Width; x++)
                {
                    row[x] = valueSelector(*(oRow + (x * pixelSize)), *(oRow + ((x * pixelSize) + 1)), *(oRow + ((x * pixelSize) + 2)));
                }
                newData[y] = row;
            }

            //unlock the bitmaps
            b.UnlockBits(bits);
            return new Mat(b.Height, b.Width, newData).T();
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

        public double SumRange(int startRow, int endRow, int startCol, int endCol)
        {
            if (startRow * endRow * startCol * endCol == 0)
            {
                return 0;
            }
            if (startRow > Rows || startCol > Cols)
            {
                return 0;
            }

            endRow = System.Math.Min(Rows, endRow);
            endCol = System.Math.Min(Cols, endCol);

            var sum = 0.0;
            for (var row = startRow; row <= endRow; row++)
            {
                for (var col = startCol; col <= endCol; col++)
                {
                    sum += this[row, col];
                }
            }
            return sum;
        }

        public double SumAll()
        => SumRange(1, Rows, 1, Cols); public double Min()
        {
            var min = double.PositiveInfinity;
            for (var row = 1; row <= Rows; row++)
            {
                for (var col = 1; col <= Cols; col++)
                {
                    min = System.Math.Min(min, this[row, col]);
                }
            }
            return min;
        }

        public double Max()
        {
            var max = double.NegativeInfinity;
            for (var row = 1; row <= Rows; row++)
            {
                for (var col = 1; col <= Cols; col++)
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


        public IEnumerable<Point> Find(Predicate<double> expr)
        {
            for (int row = 1; row <= Rows; row++)
            {
                for (int column = 1; column <= Cols; column++)
                {
                    if (expr(this[row, column]))
                    {
                        yield return new Point(column, row);
                    }
                }
            }
        }
        public double this[int row, int col]
        {
            get => _data[row - 1][col - 1];
            set
            {

                _data[row - 1][col - 1] = value;
#if DEBUG
                MatChanged?.Invoke(row, col, value);
#endif
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

        public double this[Point pos]
        {
            get => this[pos.X, pos.Y];
            set => this[pos.X, pos.Y] = value;
        }



        public void SetAll(double val)
        {
            _data = GenerateData(Rows, Cols, val);
        }

        public Mat Copy()
        => new Mat(Rows, Cols, _data);


        public Mat BinaryTresh(double tresh)
        => ProductTemplate(this, (_, __) => tresh, (v, t) => v <= t ? 0 : 1);
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

            if (other.Size == Size)
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
                if (other.Cols == 1)
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

        //public static Mat FromCV(OpenCvSharp.Mat cvMat)
        //{
        //    return new Mat(cvMat.Rows, cvMat.Cols, ResolveCvData(cvMat));
        //}

        //public static double[] ResolveCvData(OpenCvSharp.Mat m)
        //    => m.ElemSize() switch
        //    {
        //        1 when m.Channels() == 1 => GetByteArray(m),
        //        8 when m.Channels() == 1 => GetArray<double>(m),
        //        _ => throw new ArgumentException()
        //    };

        //private unsafe static double[] GetByteArray(OpenCvSharp.Mat m)
        //{
        //    var bytes = GetArray<byte>(m);
        //    return bytes.Select(Convert.ToDouble).ToArray();
        //}
        //private unsafe static double[] GetDoubleArray(OpenCvSharp.Mat m)
        //=> GetArray<double>(m);
        //private unsafe static T[] GetArray<T>(OpenCvSharp.Mat m) where T : unmanaged
        //{
        //    var buffer = new T[m.Rows * m.Cols];
        //    fixed (void* dtaPtr = &buffer[0])
        //    {
        //        var len = buffer.Length * sizeof(T);
        //        Buffer.MemoryCopy(m.DataPointer, dtaPtr, len, len);
        //        return buffer;
        //    }
        //}

        public Point Size
        => new Point(Cols, Rows);


        public Mat Pow(double power) => ProductTemplate(this, (r, c) => power, System.Math.Pow);
        public Mat Floor() => ProductTemplate(this, (r, c) => 0.0, (l, r) => System.Math.Floor(l));

        public void VConcat(Mat other)
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

        public void HConcat(Mat other)
        {
            var newcols = (Cols + other.Cols);
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
