using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class FDSUpdateMap
    {
        public static State DoFDSUpdateMap(State state, Mat newMap)
        {
            var startPos = state.StartPos;
            var endPos = state.EndPos;
            var pattern = state.Pattern;
            var ucc = state.Ucc;
            var graph = state.Graph;
            var kM = state.KM;
            var SQRT2 = Math.Sqrt(2) - 1;
            var stack = state.Stack;

            var stack2 = new SortedList<Mat, Mat>(new DStarComparator());
            var stack3 = new SortedList<Mat, Mat>(new DStarComparator());
            var map = newMap;
            var difference = (map - state.Map).ToMat();
            state.Map = map;
            var (x, y) = difference.Find(d => d == -1);
            x.GetArray<double>(out var xdata);
            x.GetArray<double>(out var ydata);

            var indices = MatExtensions.FromRows(xdata, ydata);
            for (int r = 0; r < indices.Rows; r++)
            {
                indices.Set(r, 2, 0);
                indices.Set(r, 3, 0);
            }

            var Ind = indices.T().ToMat();

            foreach (var n in indices.T().ToMat().AsMathlabEnumerable())
            {
                foreach (var s in state.Pattern.AsMathlabEnumerable())
                {
                    Ind.Add(n + s);
                }
            }


            var added = Ind.T().ToMat().UniqueRows().T().ToMat();

            (x, y) = difference.Find(d => d == 1);
            x.GetArray(out xdata);
            x.GetArray(out ydata);
            indices = MatExtensions.FromRows(xdata, ydata);
            for (int r = 0; r < indices.Rows; r++)
            {
                indices.Set(r, 2, 0);
                indices.Set(r, 3, 0);
            }

            Ind = indices.T().ToMat();

            foreach (var n in indices.T().ToMat().AsMathlabColEnumerable())
            {
                foreach (var s in state.Pattern.AsMathlabColEnumerable())
                {
                    Ind.Add(n + s);
                }
            }


            var removed = Ind.T().ToMat().UniqueRows().T().ToMat();

            foreach (var u in removed.AsMathlabColEnumerable())
            {
                foreach (var n in ucc.AsMathlabColEnumerable())
                {
                    var s = u + n;
                    insert(s, h(s, graph), graph, double.PositiveInfinity, stack, kM, startPos);
                }
            }
            foreach (var u in added.AsMathlabColEnumerable())
            {
                foreach (var n in ucc.AsMathlabColEnumerable())
                {
                    var s = (u + n).ToMat();
                    if (t(s, graph) == (double)OutcomeState.CLOSED && h(s, graph) != double.PositiveInfinity)
                    {
                        seth(s, double.PositiveInfinity);
                        s.Set(2, 0, -k(s,graph) + kM);
                        s.Set(3, 0, k(s,graph) + g(s));
                        stack.Add(s);
                        sett(s, OutcomeState.OPEN);
                    }
                }
            }

            var outState = state;
            outState.Graph = graph;
            outState.Map = map;
            outState.Stack = stack;
            return outState;
        }

        private static double t(Mat s, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            return graph[2].Get<double>(s1.Get<int>(0), s1.Get<int>(1));
        }
        private static double h(Mat s, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            return graph[0].Get<double>(s1.Get<int>(0), s1.Get<int>(1));
        }

        private static double k(Mat s, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            return graph[1].Get<double>(s1.Get<int>(0), s1.Get<int>(1));
        }

        private static double g(Mat s, Mat[] graph, Mat startPos) 
        {
            var ss = (startPos - s).Abs().T().ToMat();
            return Math.Sqrt(Math.Pow(ss.Get<double>(0), 2) + Math.Pow(ss.Get<double>(1), 2));
        }


        function out = inQ(s)
    out = graph(s(1), s(2), 3);
            end

            function sett(s, val)
    graph(s(1), s(2), 3) = val;
            end
            function setQ(s)
     graph(s(1), s(2), 3) = 1;
            end
            function rsetQ(s)
     graph(s(1), s(2), 3) = 0;
            end
            function incr(s)
    graph(s(1), s(2), 4) = graph(s(1), s(2), 4) + 1;
            end
            % -----------------------------------------------------------

            function seth(s, val)
    graph(s(1), s(2), 1) = val;
            end
            % -----------------------------------------------------------
            
            function setk(s, val)
    graph(s(1), s(2), 2) = val;
            end
            % -----------------------------------------------------------
            function out = calculateKey(x)
    out =  [
        h(x) + g(x) + kM;
% min(h(x), k(x) + g(x));
            min(h(x), k(x))
        ];
            end
            function out = testNode(s)
    out = true;
            if template(s(1), s(2)) == 1
        out = false;
            return
        end
    for n = pattern
        pos = s + n;
        if map(pos(1), pos(2)) == 0
            out = false;
            template(s(1), s(2)) = 1;
% graph(s(1), s(1), 5) = 0;
% setg(s, -1)
            return
        end
    end
end
function out = cmp(s1, s2)
    out = s1(1) < s2(1) || (s1(1) == s2(1) && s1(2) < s2(2));
            end
            function insert(s, h_new)
        t = inQ(s);
            if t == NEW
                setk(s, h_new);
        seth(s, h_new);
        else
            if t == OPEN
                setk(s, min(k(s), h_new));
            else
                setk(s, min(h(s), h_new));
            seth(s, h_new);
        end
    s(3:4) = [k(s) + kM + g(s); k(s) + g(s)];
            add(stack, s);
        sett(s, OPEN);
        end

end

end
    }
}
