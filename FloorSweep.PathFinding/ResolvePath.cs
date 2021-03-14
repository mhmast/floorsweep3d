
using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    internal class ResolvePath
    {

        private static double g(Point s, Mat[] graph)
        {
            return graph[0]._<double>(s.X, s.Y);
        }

        private static void resolve(State state)
        {
            var s = state.EndPos;

            state.Graph[0]._Set<double>(s.X, s.Y, double.PositiveInfinity);
            //var uval = g(s, state.Graph);
            double minval;
            var path = new List<Point>();
            while (s != state.StartPos)
            {
                minval = double.PositiveInfinity;
                Point? it = null;
                foreach (var n in state.Ucc)
                {

                    var u = s + n;
                    var uval = g(u, state.Graph);
                    if (uval < minval && !double.IsInfinity(uval) && uval > -1)
                    {
                        minval = uval;
                        it = n;
                    }
                }


                if (it == null)
                {
                    Console.WriteLine("there is no valid path");
                    return;
                }
                s = s+it.Value;
                state.Graph[0]._Set<double>(s.X, s.Y, double.PositiveInfinity);
                path.Add(s);

            }
            state.Path = path;
            state.Path.Reverse();

        }

        private static Mat b(Mat x, Mat[] graph)
        {
            return x.Plus(MatExtensions.FromRows(new[] { graph[5]._<double>(x.__(1), x.__(2)) }, new[] { graph[6]._<double>(x.__(1), x.__(2)) }, new[] { 0.0 }, new[] { 0.0 }));
        }

        public static void DoResolvePath(State state)
        => resolve(state);
    }
}
