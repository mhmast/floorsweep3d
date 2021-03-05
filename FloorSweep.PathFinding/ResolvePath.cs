
using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class ResolvePath
    {

        private static double g(Mat s, Mat[] graph)
        {
            return graph[0]._<double>(s.__(1), s.__(2));
        }

        private static void resolve(State state)
        {
            var s = state.EndPos;

            state.Graph[0]._Set<double>(s.__(1), s.__(2), double.PositiveInfinity);
            var uval = g(s, state.Graph);
            var minval = double.PositiveInfinity;
            var path = new List<Mat>();
            while (!s.Rows(1, 2).IsEqual(state.StartPos.Rows(1, 2)))
            {
                minval = double.PositiveInfinity;
                var it = new Mat();
                foreach (var n in state.Ucc)
                {

                    var u = s.Plus(n);
                    uval = g(u, state.Graph);
                    if (uval < minval && !double.IsInfinity(uval) && uval > -1)
                    {
                        minval = uval;
                        it = n;
                    }
                }


                if (it.Empty())
                {
                    Console.WriteLine("there is no valid path");
                    return;
                }
                s = s.Plus(it);
                state.Graph[0]._Set<double>(s.__(1), s.__(2), double.PositiveInfinity);
                path.Add(s);

            }
            state.Path = path;
            
        }

        private static Mat b(Mat x, Mat[] graph)
        {
            return x.Plus(MatExtensions.FromRows(new[] { graph[5]._<double>(x.__(1), x.__(2)) }, new[] { graph[6]._<double>(x.__(1), x.__(2)) }, new[] { 0.0 }, new[] { 0.0 }));
        }

        public static void DoResolvePath(State state)
        => resolve(state);
    }
}
