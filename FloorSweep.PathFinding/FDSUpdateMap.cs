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
            var difference = map.Minus(state.Map);
            state.Map = map;
            var (x, y) = difference.Find(d => d == -1);
            var xdata = x.DataLeftToRight<double>();
            var ydata = y.DataLeftToRight<double>();

            var indices = MatExtensions.FromCols(xdata, ydata);
            indices.SetColRange(2, 3, 0);
            
            var Ind = indices._T();

            foreach (var n in indices._T().AsMathlabColEnumerable())
            {
                foreach (var s in state.Pattern.AsMathlabColEnumerable())
                {
                    Ind.AddColumn(n.Plus(s));
                }
            }


            var added = Ind._T().UniqueRows()._T();

            (x, y) = difference.Find(d => d == 1);
            xdata = x.DataLeftToRight<double>();
            ydata = y.DataLeftToRight<double>();

            indices = MatExtensions.FromCols(xdata, ydata);
            indices.SetColRange(2, 3, 0);
            Ind = indices._T();

            foreach (var n in indices._T().AsMathlabColEnumerable())
            {
                foreach (var s in state.Pattern.AsMathlabColEnumerable())
                {
                    Ind.AddColumn(n.Plus(s));
                }
            }


            var removed = Ind._T().UniqueRows()._T();

            foreach (var u in removed.AsMathlabColEnumerable())
            {
                foreach (var n in ucc.AsMathlabColEnumerable())
                {
                    var s = u.Plus(n);
                    insert(s, h(s, graph), graph, kM, startPos, stack);
                }
            }
            foreach (var u in added.AsMathlabColEnumerable())
            {
                foreach (var n in ucc.AsMathlabColEnumerable())
                {
                    var s = u.Plus(n);
                    if (t(s, graph) == (double)OutcomeState.CLOSED && h(s, graph) != double.PositiveInfinity)
                    {
                        seth(s, double.PositiveInfinity, graph);
                        s._Set<double>(2, 0, -k(s, graph) + kM);
                        s._Set<double>(3, 0, k(s, graph) + g(s, startPos));
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
            return graph[2]._<double>(s.__(0), s.__(1));
        }
        private static double h(Mat s, Mat[] graph)
        {
            return graph[0]._<double>(s.__(0), s.__(1));
        }

        private static double k(Mat s, Mat[] graph)
        {
            return graph[1]._<double>(s.__(0), s.__(1));
        }

        private static double g(Mat s, Mat startPos)
        {
            var ss = (startPos - s).Abs().ToMat();
            return Math.Sqrt(Math.Pow(ss._<double>(0), 2) + Math.Pow(ss._<double>(1), 2));
        }

        private static void sett(Mat s, double val, Mat[] graph)
        {
            graph[2]._Set<double>(s.__(0), s.__(1), val);
        }

        private static void seth(Mat s, double val, Mat[] graph)
        {
            graph[0]._Set<double>(s.__(0), s.__(1), val);
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
                s._Set<double>(2, 0, k(s, graph) + kM + g(s, startPos));
                s._Set<double>(3, 0, k(s, graph) + g(s, startPos));
                stack.Add(s);
                sett(s, (double)OutcomeState.OPEN, graph);
            }
        }

        private static double inQ(Mat s, Mat[] graph)
        {
            return graph[2]._<double>(s.__(0), s.__(1));
        }

        private static void setk(Mat s, double val, Mat[] graph)
        {
            graph[1]._Set<double>(s.__(0), s.__(1), val);
        }


    }
}
