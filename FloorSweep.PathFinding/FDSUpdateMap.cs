﻿using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            var indices = MatExtensions.FromCols(xdata, ydata)._T().Columns().Select(c => new Point(c.__(1), c.__(2))).ToList();
            var Ind = new List<Point>(indices);

            foreach (var n in indices)
            {
                foreach (var s in state.Pattern)
                {
                    Ind.Add(n + s);
                }
            }


            var added = Ind.Distinct(Point.Comparer);//._T().UniqueRows()._T();

            (x, y) = difference.Find(d => d == 1);
            xdata = x.Data;
            ydata = y.Data;

            indices = MatExtensions.FromCols(xdata, ydata)._T().Columns().Select(c => new Point(c.__(1), c.__(2))).ToList();
            Ind = new List<Point>(indices);

            foreach (var n in indices)
            {
                foreach (var s in state.Pattern)
                {
                    Ind.Add(n + s);
                }
            }


            var removed = Ind.Distinct(Point.Comparer);

            foreach (var u in removed)
            {
                foreach (var n in ucc)
                {
                    var s = u + n;
                    insert(s, h(s, graph), graph, kM, startPos, stack);
                }
            }
            foreach (var u in added)
            {
                foreach (var n in ucc)
                {
                    var s = u + n;
                    if (t(s, graph) == (double)OutcomeState.CLOSED && h(s, graph) != double.PositiveInfinity)
                    {
                        seth(s, double.PositiveInfinity, graph);
                        stack.Add(new Point4(s, new PointD(-k(s, graph) + kM, k(s, graph) + g(s, startPos))));
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

        private static double t(Point s, Mat[] graph)
        {
            return graph[2]._<double>(s.X, s.Y);
        }
        private static double h(Point s, Mat[] graph)
        {
            return graph[0]._<double>(s.X, s.Y);
        }

        private static double k(Point s, Mat[] graph)
        {
            return graph[1]._<double>(s.X, s.Y);
        }

        private static double g(Point s, Point startPos)
        {
            var ss = (startPos - s).Abs();
            return System.Math.Sqrt(System.Math.Pow(ss.X, 2) + System.Math.Pow(ss.Y, 2));
        }

        private static void sett(Point s, double val, Mat[] graph)
        {
            graph[2]._Set<double>(s.X, s.Y, val);
        }

        private static void seth(Point s, double val, Mat[] graph)
        {
            graph[0]._Set<double>(s.X, s.Y, val);
        }

        private static void insert(Point s, double h_new, Mat[] graph, double kM, Point startPos, SortedSet<Point4> stack)
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
                
                stack.Add(new Point4(s, new PointD(k(s, graph) + kM + g(s, startPos), k(s, graph) + g(s, startPos))));
                sett(s, (double)OutcomeState.OPEN, graph);
            }
        }

        private static double inQ(Point s, Mat[] graph)
        {
            return graph[2]._<double>(s.X, s.Y);
        }

        private static void setk(Point s, double val, Mat[] graph)
        {
            graph[1]._Set<double>(s.X, s.Y, val);
        }


    }
}
