using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class ResolvePath
    {

        private static double g(Mat s, Mat[] graph)
        {
            return graph[0]._<double>(s.__(0), s.__(1));
        }

        private static Mat resolve(State state)
        {
            var s = state.EndPos;
            var @out = s.Copy(); 
            state.Graph[0]._Set<double>(s.__(0), s.__(1), double.PositiveInfinity);
            var uval = g(s, state.Graph);
            var minval = double.PositiveInfinity;
            while (!s.RowRange(0, 1).IsEqual(state.StartPos.RowRange(0, 1)))
            {
                minval = double.PositiveInfinity;
                var it = new Mat();
                foreach (var n in state.Ucc.AsMathlabColEnumerable())
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
                    return new Mat();
                }
                s = s.Plus(it);
                state.Graph[0]._Set<double>(s.__(0), s.__(1), double.PositiveInfinity);
                @out.AddColumn(s);

            }
            return @out;
        }

        private static Mat b(Mat x, Mat[] graph)
        {
            return x.Plus(MatExtensions.FromRows(new[] { graph[5]._<double>(x.__(0), x.__(1)) }, new[] { graph[6]._<double>(x.__(0), x.__(1)) }, new[] { 0.0 }, new[] { 0.0 }));
        }

        public static Mat DoResolvePath(State state)
        => resolve(state);
    }
}
