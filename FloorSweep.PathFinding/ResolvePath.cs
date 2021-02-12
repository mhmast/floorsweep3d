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
            var s1 = s.T().ToMat();
            return graph[0].Get<double>(s1.Get<int>(0), s1.Get<int>(1));
        }

        private static Mat resolve(State state)
        {
            var s = state.EndPos;
            var @out = s;
            var s1 = s.T().ToMat();
            state.Graph[0].Set(s1.Get<int>(0), s1.Get<int>(1), double.PositiveInfinity);
            var uval = g(s, state.Graph);
            var minval = double.PositiveInfinity;
            while (s.Col(0).RowRange(0, 1) != state.StartPos.Col(0).RowRange(0, 1))
            {
                minval = double.PositiveInfinity;
                var it = new Mat();
                foreach (var n in state.Ucc.AsMathlabColEnumerable())
                {

                    var u = s + n;
                    uval = g(u, state.Graph);
                    if (uval < minval && !double.IsInfinity(uval) && uval > -1)
                    {
                        minval = uval;
                        it = n;
                    }
                }
                if (it.Empty())
                {
                    Console.WriteLine(" there is no valid path");
                    return @out;
                }
                s = s + it;
                s1 = s.T().ToMat();
                state.Graph[0].Set(s1.Get<int>(0), s1.Get<int>(1), double.PositiveInfinity);
                @out = @out.T().ToMat();
                @out.Add(s);
                @out = @out.T().ToMat();

            }
            return @out;
        }

        public static Mat DoResolvePath(State state)
        => resolve(state);
    }
}
