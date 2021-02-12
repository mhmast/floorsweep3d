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

            var stack2 = new SortedSet<Mat>(new DStarComparator());
            var stack3 = new SortedSet<Mat>(new DStarComparator());
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
                    insert(s, h(s, graph), graph, kM, startPos, stack);
                }
            }
            foreach (var u in added.AsMathlabColEnumerable())
            {
                foreach (var n in ucc.AsMathlabColEnumerable())
                {
                    var s = (u + n).ToMat();
                    if (t(s, graph) == (double)OutcomeState.CLOSED && h(s, graph) != double.PositiveInfinity)
                    {
                        seth(s, double.PositiveInfinity, graph);
                        s.Set(2, 0, -k(s, graph) + kM);
                        s.Set(3, 0, k(s, graph) + g(s, graph, startPos));
                        stack.Add(s);
                        sett(s, (double)OutcomeState.OPEN, graph);
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

        private static void sett(Mat s, double val, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            graph[2].Set(s1.Get<int>(0), s.Get<int>(1), val);
        }

        private static void seth(Mat s, double val, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            graph[0].Set(s1.Get<int>(0), s1.Get<int>(1), val);
        }

        private static void insert(Mat s, double h_new, Mat[] graph, double kM, Mat startPos, SortedSet<Mat> stack)
        {
            var t = inQ(s, graph);
            if (t == (double)OutcomeState.NEW)
            {
                setk(s, h_new, graph);
                seth(s, h_new, graph);
            }
            else
            {
                if (t == (double)OutcomeState.OPEN)
                {
                    setk(s, Math.Min(k(s, graph), h_new), graph);
                }
                else
                {
                    setk(s, Math.Min(h(s, graph), h_new), graph);
                    seth(s, h_new, graph);
                }
                s.Col(0).Set(2, k(s, graph) + kM + g(s, graph, startPos));
                s.Col(0).Set(3, k(s, graph) + g(s, graph, startPos));
                stack.Add(s);
                sett(s, (double)OutcomeState.OPEN, graph);
            }
        }

        private static double inQ(Mat s, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            return graph[2].Get<double>(s1.Get<int>(0), s1.Get<int>(1));
        }

        private static void setk(Mat s, double val, Mat[] graph)
        {
            var s1 = s.T().ToMat();
            graph[1].Set(s1.Get<int>(0), s1.Get<int>(1), val);
        }


    }
}
