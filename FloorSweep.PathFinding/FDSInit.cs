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
            var radius = 10.0 / scaling;
            var mat = Mat.Zeros((int)(radius * 2), (int)(radius * 2),MatType.CV_64FC1).ToMat();
            double dista(double a, double b) => Math.Sqrt(a * a + b * b);

            var rows = new List<double[]>();

            for (int x = 0; x < 2 * radius; x++)
            {
                for (int y = 0; y < 2 * radius; y++)
                {
                    if (dista(x+1 - radius - 0.5, y+1 - radius - 0.5) > radius - 1 && dista(x+1 - radius - 0.5, y+1 - radius - 0.5) < radius)
                    {
                        mat._<double>(x, y) = 1.0;
                        rows.Add(new double[] { Math.Floor((double)x+1 - radius), Math.Floor((double)y+1 - radius), 0, 0 });
                    }
                }
            }
            var shapePattern = MatExtensions.FromRows(rows.ToArray());

            var neighbours = Mat.FromArray(new[,] {
                { 0, 1, 0, 0, },
            { -1, 0, 0, 0 },
            { 0, - 1, 0, 0 },
            { 1, 1, 0, 0 },
            { -1, - 1, 0, 0 },
            { 1, 0, 0, 0 },
            { 1, - 1, 0, 0 },
            { -1.0, 1, 0, 0 }
            }).T();

            var @out = new State();

            @out.Map = map;
            @out.StartPos = startPos.Range(0,1,1,1);
            @out.StartPos.AddBottom(Mat.Zeros(2, @out.StartPos.Cols, @out.StartPos.Type()));
            @out.EndPos = endPos.Range(0,1,1,1);
            @out.EndPos.AddBottom(Mat.Zeros(2, @out.EndPos.Cols, @out.EndPos.Type()));
            startPos = @out.StartPos;

            @out.Scaling = scaling;
            @out.Pattern = shapePattern.T();
            @out.Ucc = neighbours;
            @out.Height = @out.Map.Rows;
            @out.Width = @out.Map.Cols;
            @out.Graph = new Mat[7];
            Array.Fill(@out.Graph, Mat.Zeros(rows: @out.Height, @out.Width, MatType.CV_64F).ToMat());
            @out.Graph[0].SetAll(double.PositiveInfinity);
            @out.Graph[1].SetAll(double.PositiveInfinity);
            @out.Graph[2].SetAll(-1.0);
            @out.Graph[4].SetAll(-1.0);
            @out.KM = 0.0;
            var SQRT2 = Math.Sqrt(2) - 1;
            @out.Stack = new SortedSet<Mat>(new DStarComparator());
            @out.Graph[1]._<double>(@out.EndPos.__(0, 0), @out.EndPos.__(1, 0)) = 0;
            @out.Graph[0]._<double>(@out.EndPos.__(0, 0), @out.EndPos.__(1, 0)) = 0;
            @out.Graph[2]._<double>(@out.EndPos.__(0, 0), @out.EndPos.__(1, 0)) = 1;
            var k = (startPos - @out.EndPos).Abs().ToMat();
            var m11m21 = k.Rows(0, 1).Cols(0,0);
            var heur = SQRT2 * m11m21.Min() + m11m21.Max();
            @out.EndPos._<double>(2, 0) = heur;
            @out.EndPos._<double>(3, 0) = 0;
            @out.Stack.Add(@out.EndPos);
            return @out;
        }
    }
}