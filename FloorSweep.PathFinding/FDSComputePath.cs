using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FloorSweep.PathFinding
{
    public enum OutcomeState
    {
        NEW = -1,
        OPEN = 1,
        CLOSED = 0
    }
    public class FDSComputePath
    {

        public static State DoFdsComputePath(State state, double limit = double.PositiveInfinity)
        {
            var startPos = state.StartPos;
            var endPos = state.EndPos;
            var map = state.Map;
            var pattern = state.Pattern;
            var ucc = state.Ucc;
            var graph = state.Graph;
            var template = state.Template;
            var kM = state.KM;
            var stack = state.Stack;
            var mindist = double.PositiveInfinity;

            var vis = state.Vis;

            //var last = [];
            var found = computeShortestPath(stack, graph, limit, template, ucc, ref mindist, startPos, pattern, map, kM, vis);
            state.Exist = found;
            if (found)
            {
                seth(startPos, k(startPos, graph), graph);
            }

            state.EndPos = startPos;
            state.StartPos = endPos;
            state.Graph = graph;
            state.Stack = stack;
            state.Length = graph[1]._<double>(startPos.X, startPos.Y);
            return state;
        }


        private static bool computeShortestPath(
            PriorityQueue<Point4> stack, 
            Mat[] graph, 
            double limit, 
            Mat template, 
            IEnumerable<Point> ucc, 
            ref double mindist, 
            Point startPos, 
            IEnumerable<Point> pattern, 
            Mat map, 
            double kM, 
            Mat vis)
        {
            var terminate = false;
            var count = 0;
            var c1 = 0;
            var c2 = 0;
            var c3 = 0;
            var found = false;

            while (!terminate && stack.Count > 0)
            {

                if (count > limit)
                {
                    terminate = true;
                }
                count = count + 1;
                var x = stack.Dequeue();
                var xXY = x.XY;
                if (t(xXY, graph) == OutcomeState.CLOSED)
                {
                    continue;
                }

                var val = x.AB;
                var k_val = k(xXY, graph);
                var h_val = h(xXY, graph);
                rsetQ(xXY, graph);

                if (testNode(xXY, template, pattern, map))
                {
                    if (h_val > k_val)
                    {
                        foreach (var n in ucc)
                        {
                            var y = x.XY + n;
                            if (t(y, graph) != OutcomeState.NEW && k(xXY, graph) == k(y, graph) + 1)
                            {
                                seth(x.XY, h(y, graph) + 1, graph);
                                h_val = h(x.XY, graph);
                                setb(x.XY, y, graph);
                                c1 = c1 + 1;
                            }
                        }
                    }
                    if (k_val == h(xXY, graph) && h(xXY, graph) < mindist)
                    {
                        c2 = c2 + 1;
                        foreach (var n in ucc)
                        {
                            var y = x.XY + n;
                            if (t(y, graph) == OutcomeState.NEW || h(y, graph) > h_val + 1)
                            {

                                insert(y, h_val + 1, graph, mindist, stack, kM, startPos);
                                setb(y, xXY, graph);
                            }

                        }
                        if (xXY == startPos)
                        {
                            mindist = System.Math.Min(mindist, h(xXY, graph)) + 1;
                            return true;
                        }
                    }
                    else if (h_val < mindist || double.IsInfinity(h_val))
                    {
                        foreach (var n in ucc)
                        {
                            var y = xXY + n;

                            if (t(y, graph) == OutcomeState.NEW || (h(y, graph) != h_val + 1) && vis.__(y.X, y.Y) != 1)
                            {
                                c3 = c3 + 1;
                                vis._Set<double>(y.X, y.Y, 1);
                                insert2(y, h_val + 1, graph, mindist, kM, stack, startPos);
                                setb(y, xXY, graph);
                            }
                            else
                            {
                                if ((b(y, graph) != xXY) && h(y, graph) > h_val + 1 && t(xXY, graph) == OutcomeState.CLOSED)
                                {
                                    insert2(xXY, h_val, graph, mindist, kM, stack, startPos);
                                }
                                else if ((b(y, graph) != xXY) && h_val > h(y, graph) + 1 && t(y, graph) == OutcomeState.CLOSED && cmp(calculateKey(y, kM, graph), val))
                                {
                                    insert2(y, h(y, graph), graph, mindist, kM, stack, startPos);
                                }
                            }
                        }
                    }

                }
                else
                {

                    sett(xXY, (double)OutcomeState.CLOSED, graph);
                    setk(xXY, double.PositiveInfinity, graph);
                    seth(xXY, double.PositiveInfinity, graph);
                }


            }

            Console.WriteLine($"count: {count}");
            Console.WriteLine($" {c1}   {c2}   {c3}");
            return found;
        }

        private static OutcomeState t(Point s, Mat[] graph)
        {
            return (OutcomeState)graph[2]._<double>(s.X, s.Y);
        }

        private static double k(Point s, Mat[] graph)
        {
            return graph[1]._<double>(s.X, s.Y);
        }

        private static void setk(Point s, double val, Mat[] graph)
        {
            graph[1]._Set<double>(s.X, s.Y, val);
        }

        public static int h(Point s, Mat[] graph)
        {
            return graph[0].__(s.X, s.Y);
        }
        private static void seth(Point s, double val, Mat[] graph)
        {
            graph[0]._Set<double>(s.X, s.Y, val);
        }

        private static void rsetQ(Point s, Mat[] graph)
        {
            graph[2]._Set<double>(s.X, s.Y, 0);
        }

        private static bool testNode(Point s, Mat template, IEnumerable<Point> pattern, Mat map)
        {
            if (template.__(s.X, s.Y) == 1)
            {
                return false;
            }
            foreach (var n in pattern)
            {
                var y = n + s;
                if (map.__(y.X, y.Y) == 0)
                {
                    template._Set<double>(y.X, y.Y, 1);
                    return false;
                }
            }
            return true;
        }

        private static Point b(Point x, Mat[] graph)
        {
            return new Point(graph[5].__(x.X, x.Y), graph[6].__(x.X, x.Y)) + x;
        }

        private static OutcomeState inQ(Point s, Mat[] graph)
        {
            return (OutcomeState)graph[2].__(s.X, s.Y);
        }

        private static void setb(Point x, Point y, Mat[] graph)
        {
            var val = y - x;
            graph[5]._Set<double>(x.X, x.Y, val.X);
            graph[6]._Set<double>(x.X, x.Y, val.Y);
        }

        private static PointD calculateKey(Point x, double kM, Mat[] graph)
        {
            return new PointD(
                 h(x, graph) + g(x) + kM,
                 System.Math.Min(h(x, graph), k(x, graph))
            );
        }

        private static double g(Point s)
        {
            return System.Math.Sqrt(System.Math.Pow(s.X, 2) + System.Math.Pow(s.Y, 2));
        }

        private static void sett(Point s, double val, Mat[] graph)
        {
            graph[2]._Set<double>(s.X, s.Y, val);
        }

        //private static void setQ(Mat s, Mat[] graph)
        //{
        //    var s2 = s.T().ToMat();
        //    graph[2]._Set<double>(s2.__(1), s2.__(2), 1);
        //}

        //private static void incr(Mat s, Mat[] graph)
        //{
        //    var s2 = s.T().ToMat();
        //    graph[3]._Set<double>(s2.__(1), s2.__(2), graph[3]._<double>(s2.__(1), s2.__(2)) + 1);
        //}

        private static PointD calculateKey2(Point x, Mat[] graph, double kM) => new PointD(k(x, graph) + kM, System.Math.Min(h(x, graph), k(x, graph)));

        public static void insert(Point s, double h_new, Mat[] graph, double mindist, PriorityQueue<Point4> stack, double kM, Point startPos)
        {
            var t = inQ(s, graph);
            if (t == OutcomeState.NEW && h_new < mindist)
            {
                setk(s, h_new, graph);
            }
            else
            {
                if (t == OutcomeState.OPEN && h_new < mindist)
                {
                    setk(s, System.Math.Min(k(s, graph), h_new), graph);
                }
                else if (h_new < mindist)
                {

                    setk(s, System.Math.Min(h(s, graph), h_new), graph);
                }
            }
            seth(s, h_new, graph);
            var key = calculateKey(s, kM, graph);
            stack.Queue(new Point4(s, key, startPos));
            sett(s, (double)OutcomeState.OPEN, graph);

        }


        private static bool cmp(PointD s1, PointD s2)
        {
            return s1.X < s2.X || (s1.X == s2.X && s1.Y < s2.Y);
        }
        private static void insert2(Point s, double h_new, Mat[] graph, double mindist, double kM, PriorityQueue<Point4> stack, Point startPos)
        {
            var t = inQ(s, graph);
            if (t == OutcomeState.NEW && h_new < mindist)
            {
                setk(s, h_new, graph);
            }
            else
            {
                if (t == OutcomeState.OPEN && h_new < mindist)
                {
                    setk(s, System.Math.Min(k(s, graph), h_new), graph);
                }
                else if (h_new < mindist)
                {
                    setk(s, System.Math.Min(h(s, graph), h_new), graph);
                }
            }
            seth(s, h_new, graph);
            var key2 = calculateKey2(s, graph, kM);
            stack.Queue(new Point4(s, key2, startPos));
            sett(s, (double)OutcomeState.OPEN, graph);

        }
    }
}
