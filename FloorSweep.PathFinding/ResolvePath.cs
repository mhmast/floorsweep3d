
using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    internal class ResolvePath
    {

        private static double g(Point s, Mat[] graph) => graph[0][s];

        private static void resolve(State state)
        {
            var s = state.EndPos;

            state.Graph[0]._Set<double>(s.X, s.Y, double.PositiveInfinity);
            //var uval = g(s, state.Graph);
            var margin = new Point(5, 5);
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
                path.Add((s - margin)*state.Scaling);

            }
            state.Path = path;
            state.Path.Reverse();

        }


        public static void DoResolvePath(State state)
        => resolve(state);
    }
}
