using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var mat = Mat.Zeros((int)(radius * 2), (int)(radius * 2)).ToMat();
            double dista(double a, double b) => System.Math.Sqrt(a * a + b * b);

            //var rows = new List<double[]>();
            var shapePattern = new List<Point>();
            for (int x = 1; x <= 2 * radius; x++)
            {
                for (int y = 1; y <= 2 * radius; y++)
                {
                    if (dista(x - radius - 0.5, y - radius - 0.5) > radius - 1 && dista(x - radius - 0.5, y - radius - 0.5) < radius)
                    {
                        //mat._Set<double>(x, y, 1.0);
                        shapePattern.Add(new Point((int)System.Math.Floor(x - radius), (int)System.Math.Floor(y - radius)));
                    }
                }
            }
            //var shapePattern = MatExtensions.FromRows(rows.ToArray());

            var neighbours = new List<Point> {
                new Point( 0, 1),
                new Point( -1, 0 ),
                new Point( 0, -1 ),
                new Point( 1, 1),
                new Point( -1, -1),
                new Point( 1, 0),
                new Point( 1, -1 ),
                new Point( -1, 1 )
            };

            var @out = new State();

            @out.Map = map;
            @out.StartPos = startPos;
            //  @out.StartPos.AddBottom(Mat.Zeros(2, @out.StartPos.Cols));
            @out.EndPos = endPos;
            //            @out.EndPos.AddBottom(Mat.Zeros(2, @out.EndPos.Cols));
            startPos = @out.StartPos;

            @out.Scaling = scaling;
            @out.Pattern = shapePattern;//.T();
            @out.Ucc = neighbours;

            @out.Graph = new Mat[7];
            for (int i = 0; i < 7; i++)
            {
                @out.Graph[i] = Mat.Zeros(map.Rows, map.Cols).ToMat();
            }
            @out.Graph[0].SetAll(double.PositiveInfinity);
            @out.Graph[1].SetAll(double.PositiveInfinity);
            @out.Graph[2].SetAll(-1.0);
            @out.Graph[4].SetAll(-1.0);
            @out.KM = 0.0;
            var SQRT2 = System.Math.Sqrt(2) - 1;

            @out.Graph[1]._Set<double>(@out.EndPos.X, @out.EndPos.Y, 0);
            @out.Graph[0]._Set<double>(@out.EndPos.X, @out.EndPos.Y, 0);
            @out.Graph[2]._Set<double>(@out.EndPos.X, @out.EndPos.Y, 1);
            var k = (startPos - @out.EndPos).Abs();
            var heur = SQRT2 * k.Min() + k.Max();
            @out.Stack = new SortedSet<Point4>(new DStarComparator());
            @out.Stack.Add(new Point4(@out.EndPos, new PointD(heur, 0), endPos));
            @out.Image = data.Image;
            //  @out.Path = Mat.Zeros(map.Width, map.Height, MatType.CV_64FC1);
            return @out;
        }
    }
}