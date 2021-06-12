using FloorSweep.Math;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{

    internal class FDSComputePath
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
            var found = ComputeShortestPath(stack, graph, limit, template, ucc, ref mindist, startPos, pattern, map, kM, vis);

            state.Exist = found;
            if (found)
            {
                SetH(startPos, K(startPos, graph), graph);
            }

            state.EndPos = startPos;
            state.StartPos = endPos;
            state.Graph = graph;
            state.Stack = stack;
            state.Length = graph[1][startPos];
            return state;
        }


        private static bool ComputeShortestPath(
            PriorityQueue<Node> stack,
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
                count++;
                var x = stack.Dequeue();
                var xXY = x.XY;
                if (T(xXY, graph) == OutcomeState.CLOSED)
                {
                    continue;
                }

                var val = x.AB;
                var k_val = K(xXY, graph);
                var h_val = H(xXY, graph);
                ResetQ(xXY, graph);

                if (TestNode(xXY, template, pattern, map))
                {
                    if (h_val > k_val)
                    {
                        foreach (var n in ucc)
                        {
                            var y = x.XY + n;
                            if (T(y, graph) != OutcomeState.NEW && K(xXY, graph) == K(y, graph) + 1)
                            {
                                SetH(x.XY, H(y, graph) + 1, graph);
                                h_val = H(x.XY, graph);
                                SetB(x.XY, y, graph);
                                c1++;
                            }
                        }
                    }
                    if (k_val == H(xXY, graph) && H(xXY, graph) < mindist)
                    {
                        c2++;
                        foreach (var n in ucc)
                        {
                            var y = x.XY + n;
                            if (T(y, graph) == OutcomeState.NEW || H(y, graph) > h_val + 1)
                            {

                                Insert(y, h_val + 1, graph, mindist, stack, kM, startPos);
                                SetB(y, xXY, graph);
                            }

                        }
                        if (xXY == startPos)
                        {
                            mindist = System.Math.Min(mindist, H(xXY, graph)) + 1;
                            return true;
                        }
                    }
                    else if (h_val < mindist || double.IsInfinity(h_val))
                    {
                        foreach (var n in ucc)
                        {
                            var y = xXY + n;

                            if (T(y, graph) == OutcomeState.NEW || (H(y, graph) != h_val + 1) && vis[y] != 1)
                            {
                                c3++;

                                vis[y.X, y.Y] = 1;
                                Insert2(y, h_val + 1, graph, mindist, kM, stack, startPos);
                                SetB(y, xXY, graph);
                            }
                            else
                            {
                                if ((B(y, graph) != xXY) && H(y, graph) > h_val + 1 && T(xXY, graph) == OutcomeState.CLOSED)
                                {
                                    Insert2(xXY, h_val, graph, mindist, kM, stack, startPos);
                                }
                                else if ((B(y, graph) != xXY) && h_val > H(y, graph) + 1 && T(y, graph) == OutcomeState.CLOSED && Cmp(CalculateKey(y, kM, graph), val))
                                {
                                    Insert2(y, H(y, graph), graph, mindist, kM, stack, startPos);
                                }
                            }
                        }
                    }

                }
                else
                {

                    SetT(xXY, (double)OutcomeState.CLOSED, graph);
                    SetK(xXY, double.PositiveInfinity, graph);
                    SetH(xXY, double.PositiveInfinity, graph);
                }


            }

            Console.WriteLine($"count: {count}");
            Console.WriteLine($" {c1}   {c2}   {c3}");
            return found;
        }

        private static OutcomeState T(Point s, Mat[] graph) => (OutcomeState)graph[2][s];

        private static double K(Point s, Mat[] graph) => graph[1][s];

        private static void SetK(Point s, double val, Mat[] graph)
        {
            graph[1][s.X, s.Y] = val;
        }

        public static int H(Point s, Mat[] graph) => (int)graph[0][s];
        private static void SetH(Point s, double val, Mat[] graph)
        {
            graph[0][s.X, s.Y] = val;
        }

        private static void ResetQ(Point s, Mat[] graph)
        {
            graph[2][s.X, s.Y] = 0;
        }

        private static bool TestNode(Point s, Mat template, IEnumerable<Point> pattern, Mat map)
        {
            if (template[s] == 1)
            {
                return false;
            }
            foreach (var n in pattern)
            {
                var y = n + s;
                if (map[y] == 0)
                {
                    template[y.X, y.Y] = 1;
                    return false;
                }
            }
            return true;
        }

        private static Point B(Point x, Mat[] graph) => new Point((int)graph[5][x], (int)graph[6][x]) + x;

        private static OutcomeState InQ(Point s, Mat[] graph) => (OutcomeState)graph[2][s];

        private static void SetB(Point x, Point y, Mat[] graph)
        {
            var val = y - x;
            graph[5][x.X, x.Y] = val.X;
            graph[6][x.X, x.Y] = val.Y;
        }

        private static PointD CalculateKey(Point x, double kM, Mat[] graph)
        {
            return new PointD(
                 H(x, graph) + G(x) + kM,
                 System.Math.Min(H(x, graph), K(x, graph))
            );
        }

        private static double G(Point s)
        {
            return System.Math.Sqrt(System.Math.Pow(s.X, 2) + System.Math.Pow(s.Y, 2));
        }

        private static void SetT(Point s, double val, Mat[] graph)
        {
            graph[2][s.X, s.Y] = val;
        }

        private static PointD CalculateKey2(Point x, Mat[] graph, double kM) => new PointD(K(x, graph) + kM, System.Math.Min(H(x, graph), K(x, graph)));

        public static void Insert(Point s, double h_new, Mat[] graph, double mindist, PriorityQueue<Node> stack, double kM, Point startPos)
        {
            var t = InQ(s, graph);
            if (t == OutcomeState.NEW && h_new < mindist)
            {
                SetK(s, h_new, graph);
            }
            else
            {
                if (t == OutcomeState.OPEN && h_new < mindist)
                {
                    SetK(s, System.Math.Min(K(s, graph), h_new), graph);
                }
                else if (h_new < mindist)
                {

                    SetK(s, System.Math.Min(H(s, graph), h_new), graph);
                }
            }
            SetH(s, h_new, graph);
            var key = CalculateKey(s, kM, graph);
            stack.Queue(new Node(s, key, startPos));
            SetT(s, (double)OutcomeState.OPEN, graph);

        }


        private static bool Cmp(PointD s1, PointD s2)
        {
            return s1.X < s2.X || (s1.X == s2.X && s1.Y < s2.Y);
        }
        private static void Insert2(Point s, double h_new, Mat[] graph, double mindist, double kM, PriorityQueue<Node> stack, Point startPos)
        {
            var t = InQ(s, graph);
            if (t == OutcomeState.NEW && h_new < mindist)
            {
                SetK(s, h_new, graph);
            }
            else
            {
                if (t == OutcomeState.OPEN && h_new < mindist)
                {
                    SetK(s, System.Math.Min(K(s, graph), h_new), graph);
                }
                else if (h_new < mindist)
                {
                    SetK(s, System.Math.Min(H(s, graph), h_new), graph);
                }
            }
            SetH(s, h_new, graph);
            var key2 = CalculateKey2(s, graph, kM);
            stack.Queue(new Node(s, key2, startPos));
            SetT(s, (double)OutcomeState.OPEN, graph);

        }
    }
}
