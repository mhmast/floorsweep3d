﻿using FloorSweep.Math;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class FDSInit
    {
        public static State DoFDSInit(Point start, Point end, MapData data, int scaling, State state = null)
        {
            var map = data.Map;
            var startPos = (start + data.BorderThickness)/scaling;
            var endPos = (end + data.BorderThickness)/scaling;
            var radius = 10.0 / scaling;
            //var mat = Mat.Zeros((int)(radius * 2), (int)(radius * 2)).ToMat();
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

            var @out = state ?? new State();

            @out.Map = map;
            @out.StartPos = startPos;
            //  @out.StartPos.AddBottom(Mat.Zeros(2, @out.StartPos.Cols));
            @out.EndPos = endPos;
            //            @out.EndPos.AddBottom(Mat.Zeros(2, @out.EndPos.Cols));
            startPos = @out.StartPos;

            @out.Scaling = scaling;
            @out.Pattern = shapePattern;//.T();
            @out.Ucc = neighbours;

            if (@out.Graph == null)
            {
                @out.Graph = new Mat[] {
                    new Mat(map.Rows, map.Cols,double.PositiveInfinity),
                    new Mat(map.Rows, map.Cols,double.PositiveInfinity),
                    new Mat(map.Rows, map.Cols,-1),
                    new Mat(map.Rows, map.Cols,0),
                    new Mat(map.Rows, map.Cols,-1),
                    new Mat(map.Rows, map.Cols,0),
                    new Mat(map.Rows, map.Cols,0),
                };
            }
            else
            {
                @out.Graph[0].SetAll(double.PositiveInfinity);
                @out.Graph[1].SetAll(double.PositiveInfinity);
                @out.Graph[2].SetAll(-1.0);
                @out.Graph[3].SetAll(0);
                @out.Graph[4].SetAll(-1.0);
                @out.Graph[5].SetAll(0);
                @out.Graph[6].SetAll(0);
            }
            @out.KM = 0.0;
            var SQRT2 = System.Math.Sqrt(2) - 1;

            @out.Graph[1][@out.EndPos.X, @out.EndPos.Y] = 0;
            @out.Graph[0][@out.EndPos.X, @out.EndPos.Y] = 0;
            @out.Graph[2][@out.EndPos.X, @out.EndPos.Y] = 1;
            var k = (startPos - @out.EndPos).Abs();
            var heur = SQRT2 * k.Min() + k.Max();
            @out.Stack = new PriorityQueue<Node>();
            @out.Stack.Queue(new Node(@out.EndPos, new PointD(heur, 0), endPos));
            @out.Image = data.OriginalImage;
            //  @out.Path = Mat.Zeros(map.Width, map.Height, MatType.CV_64FC1);
            return @out;
        }
    }
}