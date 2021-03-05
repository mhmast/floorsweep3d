using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var SQRT2 = System.Math.Sqrt(2) - 1;
            var stack = state.Stack;

            var map = newMap;
            var difference = map.Minus(state.Map);
            state.Map = map;
            var (x, y) = difference.Find(d => d == -1);
            var xdata = x.Data;
            var ydata = y.Data;

            var indices = MatExtensions.FromCols(xdata, ydata);
            indices.SetColRange(3, 4, 0);
            
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
            xdata = x.Data;
            ydata = y.Data;

            indices = MatExtensions.FromCols(xdata, ydata);
            indices.SetColRange(3, 4, 0);
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
                foreach (var n in ucc)
                {
                    var s = u.Plus(n);
                    insert(s, h(s, graph), graph, kM, startPos, stack);
                }
            }
            foreach (var u in added.AsMathlabColEnumerable())
            {
                foreach (var n in ucc)
                {
                    var s = u.Plus(n);
                    if (t(s, graph) == (double)OutcomeState.CLOSED && h(s, graph) != double.PositiveInfinity)
                    {
                        seth(s, double.PositiveInfinity, graph);
                        s._Set<double>(3, 1, -k(s, graph) + kM);
                        s._Set<double>(4, 1, k(s, graph) + g(s, startPos));
                        if (s._<double>(2, 1) == 2)
                        {
                            Debugger.Break();
                        }
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
            return graph[2]._<double>(s.__(1), s.__(2));
        }
        private static double h(Mat s, Mat[] graph)
        {
            return graph[0]._<double>(s.__(1), s.__(2));
        }

        private static double k(Mat s, Mat[] graph)
        {
            return graph[1]._<double>(s.__(1), s.__(2));
        }

        private static double g(Mat s, Mat startPos)
        {
            var ss = (startPos.Minus(s)).Abs().ToMat();
            return System.Math.Sqrt(System.Math.Pow(ss._<double>(0), 2) + System.Math.Pow(ss._<double>(1), 2));
        }

        private static void sett(Mat s, double val, Mat[] graph)
        {
            graph[2]._Set<double>(s.__(1), s.__(2), val);
        }

        private static void seth(Mat s, double val, Mat[] graph)
        {
            graph[0]._Set<double>(s.__(1), s.__(2), val);
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
                    setk(s, System.Math.Min(k(s, graph), h_new), graph);
                }
                else
                {
                    setk(s, System.Math.Min(h(s, graph), h_new), graph);
                    seth(s, h_new, graph);
                }
                s._Set<double>(3, 1, k(s, graph) + kM + g(s, startPos));
                s._Set<double>(4, 1, k(s, graph) + g(s, startPos));
                if (s._<double>(2, 1) == 2)
                {
                    Debugger.Break();
                }
                stack.Add(s);
                sett(s, (double)OutcomeState.OPEN, graph);
            }
        }

        private static double inQ(Mat s, Mat[] graph)
        {
            return graph[2]._<double>(s.__(1), s.__(2));
        }

        private static void setk(Mat s, double val, Mat[] graph)
        {
            graph[1]._Set<double>(s.__(1), s.__(2), val);
        }


    }
}
