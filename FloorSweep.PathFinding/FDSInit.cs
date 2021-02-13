using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class FDSInit
    {
        public static State DoFDSInit(MapData data, int scaling)
        {
            var map = data.Map;
            var startPos = data.Start;
            var endPos = data.Target;
            var radius = 10 / scaling;
            var mat = Mat.Zeros(radius * 2, radius * 2).ToMat();
            double dista(double a, double b) => Math.Sqrt(a * a + b * b);

            var shapePattern = Mat.Zeros(rows: 0, 4,MatType.CV_64FC1).ToMat();

            for (int x = 0; x < 2 * radius; x++)
            {
                for (int y = 0; y < 2 * radius; y++)
                {
                    if (dista(x - radius - 0.5, y - radius - 0.5) > radius - 1 && dista(x - radius - 0.5, y - radius - 0.5) < radius)
                    {
                        mat.Set(x, y, 1);
                        shapePattern.AddBottom(MatExtensions.FromRows(new double[][]{ new double[]{ x - radius, y - radius, 0, 0} }));
                    }
                }
            }
            var neighbours = Mat.FromArray(new[,] {
                { 0, 1, 0, 0, },
            { -1, 0, 0, 0 },
            { 0, - 1, 0, 0 },
            { 1, 1, 0, 0 },
            { -1, - 1, 0, 0 },
            { 1, 0, 0, 0 },
            { 1, - 1, 0, 0 },
            { -1.0, 1, 0, 0 }
            }).ComplexConjugate();

            var @out = new State();

            @out.Map = map;
            @out.StartPos = startPos.Col(1).RowRange(0, 1).Inv().ToMat();
            @out.StartPos.AddBottom(Mat.Zeros(2, @out.StartPos.Cols, @out.StartPos.Type()));
            @out.EndPos = endPos.Col(1).RowRange(0, 1);
            @out.EndPos.AddBottom(Mat.Zeros(2, @out.EndPos.Cols, @out.EndPos.Type()));
            startPos = @out.StartPos;

            @out.Scaling = scaling;
            @out.Pattern = shapePattern.ComplexConjugate();
            @out.Ucc = neighbours;
            @out.Height = @out.Map.Rows;
            @out.Width = @out.Map.Cols;
            @out.Graph = new Mat[7];
            Array.Fill(@out.Graph, Mat.Zeros(rows:@out.Height, @out.Width, MatType.CV_64F).ToMat());
            @out.Graph[0].SetAll(double.PositiveInfinity);
            @out.Graph[1].SetAll(double.PositiveInfinity);
            @out.Graph[2].SetAll(-1.0);
            @out.Graph[4].SetAll(-1.0);
            @out.KM = 0.0;
            var SQRT2 = Math.Sqrt(2) - 1;
            @out.Stack = new SortedSet<Mat>(new DStarComparator());
            @out.Graph[1].Set(@out.EndPos.At<int>(0,0), @out.EndPos.At<int>(1, 0), 0);
            @out.Graph[0].Set(@out.EndPos.At<int>(0,0), @out.EndPos.At<int>(1, 0), 0);
            @out.Graph[2].Set(@out.EndPos.At<int>(0,0), @out.EndPos.At<int>(1, 0), 1);
            var k = (startPos - @out.EndPos).Abs().ToMat();
            var m11m21 = k.RowRange(0, 1).Col(0);
            var heur = SQRT2 * m11m21.Min() + m11m21.Max();
            @out.EndPos.Set(2,0,heur);
            @out.EndPos.Set(3,0,0);
            @out.Stack.Add(@out.EndPos);
            return @out;
        }
    }
}