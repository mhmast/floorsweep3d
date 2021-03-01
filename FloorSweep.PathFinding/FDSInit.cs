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
            var mat = Mat.Zeros((int)(radius * 2), (int)(radius * 2), MatType.CV_64FC1).ToMat();
            double dista(double a, double b) => Math.Sqrt(a * a + b * b);

            var rows = new List<double[]>();

            for (int x = 1; x <= 2 * radius; x++)
            {
                for (int y = 1; y <= 2 * radius; y++)
                {
                    if (dista(x - radius - 0.5, y - radius - 0.5) > radius - 1 && dista(x - radius - 0.5, y - radius - 0.5) < radius)
                    {
                        mat._Set<double>(x, y, 1.0);
                        rows.Add(new double[] { Math.Floor((double)x - radius), Math.Floor((double)y - radius), 0, 0 });
                    }
                }
            }
            var shapePattern = MatExtensions.FromRows(rows.ToArray());

            var neighbours = MatExtensions.FromRows(
          new[] { 0.0, 1, 0, 0, },
          new[] { -1.0, 0, 0, 0 },
          new[] { 0.0, -1, 0, 0 },
          new[] { 1.0, 1, 0, 0 },
          new[] { -1.0, -1, 0, 0 },
          new[] { 1.0, 0, 0, 0 },
          new[] { 1.0, -1, 0, 0 },
          new[] { -1.0, 1, 0, 0 }
          );

            var @out = new State();

            @out.Map = map;
            @out.StartPos = startPos.Range(1, 2, 2, 2);
            @out.StartPos.AddBottom(Mat.Zeros(2, @out.StartPos.Cols, @out.StartPos.Type()));
            @out.EndPos = endPos.Range(1, 2, 2, 2);
            @out.EndPos.AddBottom(Mat.Zeros(2, @out.EndPos.Cols, @out.EndPos.Type()));
            startPos = @out.StartPos;

            @out.Scaling = scaling;
            @out.Pattern = shapePattern.T();
            @out.Ucc = neighbours;

            @out.Graph = new Mat[7];
            Array.Fill(@out.Graph, Mat.Zeros(rows: @out.Height, @out.Width, MatType.CV_64F).ToMat());
            @out.Graph[0].SetAll(double.PositiveInfinity);
            @out.Graph[1].SetAll(double.PositiveInfinity);
            @out.Graph[2].SetAll(-1.0);
            @out.Graph[4].SetAll(-1.0);
            @out.KM = 0.0;
            var SQRT2 = Math.Sqrt(2) - 1;
            @out.Stack = new SortedSet<Mat>(new DStarComparator());
            @out.Graph[1]._Set<double>(@out.EndPos.__(1, 1), @out.EndPos.__(2, 1), 0);
            @out.Graph[0]._Set<double>(@out.EndPos.__(1, 1), @out.EndPos.__(2, 1), 0);
            @out.Graph[2]._Set<double>(@out.EndPos.__(1, 1), @out.EndPos.__(2, 1), 1);
            var k = (startPos - @out.EndPos).Abs().ToMat();
            var m11m21 = k.Rows(1, 2).Cols(1, 1);
            var heur = SQRT2 * m11m21.Min() + m11m21.Max();
            @out.EndPos._Set<double>(3, 1, heur);
            @out.EndPos._Set<double>(4, 1, 0);
            @out.Stack.Add(@out.EndPos);
            return @out;
        }
    }
}