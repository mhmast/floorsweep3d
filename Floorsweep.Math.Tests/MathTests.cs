using FloorSweep.Math;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Floorsweep.Math.Tests
{
    public class MathTests
    {

        [Test]
        public void Mat_ShouldInitCorrectly()
        {
            const int cols = 10, rows = 15;
            var mat = new Mat(rows, cols);
            mat.Rows.Should().Be(rows);
            mat.Cols.Should().Be(cols);
            mat.Data.All(a => a == 0.0).Should().BeTrue();
            mat.Data.Length.Should().Be(rows * cols);
        }

        [Test]
        public void Zeros_ShouldInitCorrectly()
        {
            const int cols = 10, rows = 15;
            var mat = Mat.Zeros(rows, cols);
            mat.Rows.Should().Be(rows);
            mat.Cols.Should().Be(cols);
            mat.Data.All(a => a == 0.0).Should().BeTrue();
            mat.Data.Length.Should().Be(rows * cols);
        }

        [Test]
        public void Mat_ShouldInitCorrectly_WithData()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat.Data.SequenceEqual(data).Should().BeTrue();
        }

        [Test]
        public void Mat_Empty_Should_Be_True_On_No_Data()
        {
            var mat = new Mat();
            mat.Empty().Should().BeTrue();
        }

        [Test]
        public void Mat_Empty_Should_Be_False_On_Data()
        {
            var mat = new Mat(10, 10);
            mat.Empty().Should().BeFalse();
        }

        [Test]
        public void Mat_SumAll_Should_Sum()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat.SumAll().Should().Be(10.0);
        }

        [Test]
        public void Mat_Min_Should_Return_Smallest()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat.Min().Should().Be(1.0);
        }

        [Test]
        public void Mat_Max_Should_Return_Largest()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat.Max().Should().Be(4.0);
        }

        [Test]
        public void Mat_Index_Should_Get()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat[1, 1].Should().Be(1.0);
            mat[1, 2].Should().Be(2.0);
            mat[2, 1].Should().Be(3.0);
            mat[2, 2].Should().Be(4.0);
        }

        [Test]
        public void Mat_Index_Should_Set()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat[1, 1] = 7.0;
            mat[1, 1].Should().Be(7.0);
        }

        [Test]
        public void Mat_Pos_Index_Should_Get()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat[1].Should().Be(1.0);
            mat[2].Should().Be(2.0);
            mat[3].Should().Be(3.0);
            mat[4].Should().Be(4.0);
        }

        [Test]
        public void Mat_Pos_Index_Should_Set()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat[2] = 7.0;
            mat[2].Should().Be(7.0);
        }

        [Test]
        public void Mat_SetAll_Should_Set_All()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            mat.SetAll(7.0);
            mat.Data.All(a => a == 7.0).Should().BeTrue();
        }
        [Test]
        public void Mat_Copy_Should_Copy()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var copy = mat.Copy();
            copy.Data.SequenceEqual(mat.Data).Should().BeTrue();
            copy.Rows.Should().Be(mat.Rows);
            copy.Cols.Should().Be(mat.Cols);
        }

        [Test]
        public void Mat_Plus_Should_Plus()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var plus = mat.Plus(2);
            plus.Data.SequenceEqual(new double[] { 3, 4, 5, 6 }).Should().BeTrue();
        }
        [Test]
        public void Mat_Plus_Should_Plus_VectorH()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var plus = mat.Plus(new Mat(1, 2, new double[] { 1, 2 }));
            plus.Data.SequenceEqual(new double[] { 2, 4, 4, 6 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Plus_Should_Plus_VectorV()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var plus = mat.Plus(new Mat(2, 1, new double[] { 1, 2 }));
            plus.Data.SequenceEqual(new double[] { 2, 3, 5, 6 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Plus_Should_Plus_Mat()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var min = mat.Plus(new Mat(2, 2, new double[] { 1, 2, 3, 4 }));
            min.Data.SequenceEqual(new double[] { 2, 4, 6, 8 }).Should().BeTrue();
        }
        
        [Test]
        public void Mat_Minus_Should_Min()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var plus = mat.Minus(2);
            plus.Data.SequenceEqual(new double[] { -1, 0, 1, 2 }).Should().BeTrue();
        }
        [Test]
        public void Mat_Minus_Should_Min_VectorH()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var min = mat.Minus(new Mat(1, 2, new double[] { 1, 2 }));
            min.Data.SequenceEqual(new double[] { 0, 0, 2, 2 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Minus_Should_Min_VectorV()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var min = mat.Minus(new Mat(2, 1, new double[] { 1, 2 }));
            min.Data.SequenceEqual(new double[] { 0, 1, 1, 2 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Minus_Should_Min_Mat()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var mul = mat.Minus(new Mat(2, 2, new double[] { 1, 2, 3, 4 }));
            mul.Data.SequenceEqual(new double[] { 0, 0, 0, 0 }).Should().BeTrue();
        }
        
        [Test]
        public void Mat_Mul_Should_Mul()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var plus = mat.Mul(2);
            plus.Data.SequenceEqual(new double[] { 2, 4, 6, 8 }).Should().BeTrue();
        }
        [Test]
        public void Mat_Mul_Should_Mul_VectorH()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var mul = mat.Mul(new Mat(1, 2, new double[] { 1, 2 }));
            mul.Data.SequenceEqual(new double[] { 1, 4, 3, 8 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Mul_Should_Mul_VectorV()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var mul = mat.Mul(new Mat(2, 1, new double[] { 1, 2 }));
            mul.Data.SequenceEqual(new double[] { 1, 2, 6, 8 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Mul_Should_Mul_Mat()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var mul = mat.Mul(new Mat(2, 2, new double[] { 1, 2, 3, 4 }));
            mul.Data.SequenceEqual(new double[] { 1, 4, 9, 16 }).Should().BeTrue();
        }
        
        [Test]
        public void Mat_Div_Should_Div()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var div = mat.Div(2);
            div.Data.SequenceEqual(new double[] { 0.5, 1, 1.5, 2 }).Should().BeTrue();
        }
        [Test]
        public void Mat_Div_Should_Div_VectorH()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var div = mat.Div(new Mat(1, 2, new double[] { 1, 2 }));
            div.Data.SequenceEqual(new double[] { 1, 1, 3, 2 }).Should().BeTrue();
        }

        [Test]
        public void MatDiv_Should_Div_VectorV()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var div = mat.Div(new Mat(2, 1, new double[] { 1, 2 }));
            div.Data.SequenceEqual(new double[] { 1, 2, 1.5, 2 }).Should().BeTrue();
        }

        [Test]
        public void Mat_Div_Should_Div_Mat()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var div = mat.Div(new Mat(2, 2, new double[] { 1, 2, 3, 4 }));
            div.Data.SequenceEqual(new double[] { 1, 1, 1, 1 }).Should().BeTrue();
        }
        
        [Test]
        public void Mat_Pow_Should_Pow_Mat()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data);
            var pow = mat.Pow(2);
            pow.Data.SequenceEqual(new double[] { 1, 4, 9, 16 }).Should().BeTrue();
        }
        
        [Test]
        public void Mat_Floor_Should_Floor_Mat()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1.1, 2.2, 3.3, 4.4 };
            var mat = new Mat(rows, cols, data);
            var floor = mat.Floor();
            floor.Data.SequenceEqual(new double[] { 1, 2, 3, 4 }).Should().BeTrue();
        }

        [Test]
        public void Mat_T_Should_Transpose()
        {
            const int cols = 2, rows = 3;
            var data = new double[] { 1, 2, 3, 4, 5, 6 };
            var mat = new Mat(rows, cols, data).T();
            mat.Rows.Should().Be(cols);
            mat.Cols.Should().Be(rows);
            mat[1, 1].Should().Be(1);
            mat[1, 2].Should().Be(3);
            mat[1, 3].Should().Be(5);
            mat[2, 1].Should().Be(2);
            mat[2, 2].Should().Be(4);
            mat[2, 3].Should().Be(6);
        }
        
        [Test]
        public void Mat_Point_Should_Return_Point()
        {
            const int cols = 2, rows = 3;
            var data = new double[] { 1, 2, 3, 4, 5, 6 };
            var point = new Mat(rows, cols, data).Point(2,2);
            point.X.Should().Be(4);
            point.Y.Should().Be(6);
        }
        
        
        [Test]
        public void Mat_Size_Should_Return_Size()
        {
            const int cols = 2, rows = 3;
            var size = new Mat(rows, cols).Size();
            size.Height.Should().Be(rows);
            size.Width.Should().Be(cols);
        }

        [Test]
        public void Mat_Range_Should_GetRange()
        {
            const int cols = 2, rows = 3;
            var data = new double[] { 1, 2, 3, 4, 5, 6 };
            var mat = new Mat(rows, cols, data).Range(1, 2, 1, 2);
            mat.Rows.Should().Be(2);
            mat.Cols.Should().Be(2);
            mat[1, 1].Should().Be(1);
            mat[1, 2].Should().Be(2);
            mat[2, 1].Should().Be(3);
            mat[2, 2].Should().Be(4);
        }

        [Test]
        public void Mat_Range_Should_GetRange_Larger()
        {
            const int cols = 2, rows = 2;
            var data = new double[] { 1, 2, 3, 4 };
            var mat = new Mat(rows, cols, data).Range(1, 3, 1, 3);
            mat.Rows.Should().Be(3);
            mat.Cols.Should().Be(3);
            mat[1, 1].Should().Be(1);
            mat[1, 2].Should().Be(2);
            mat[1, 3].Should().Be(0);
            mat[2, 1].Should().Be(3);
            mat[2, 2].Should().Be(4);
            mat[2, 3].Should().Be(0);
            mat[3, 1].Should().Be(0);
            mat[3, 2].Should().Be(0);
            mat[3, 3].Should().Be(0);
        }

        [Test]
        public void Mat_Columns_Should_Return_Columns()
        {
            const int cols = 2, rows = 3;
            var data = new double[] { 1, 2, 3, 4, 5, 6 };
            var columns = new Mat(rows, cols, data).Columns().ToList();
            columns.Count.Should().Be(2);
            columns[0].Data.SequenceEqual(new[] { 1.0, 3, 5 }).Should().BeTrue();
            columns[1].Data.SequenceEqual(new[] { 2.0, 4, 6 }).Should().BeTrue();
        }

        [Test]
        public void Mat_ToMat_Should_ReturnThis()
        {
            var mat = new Mat(2, 2);
            mat.ToMat().Should().Be(mat);
        }

    }
}