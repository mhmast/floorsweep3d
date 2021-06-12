using FloorSweep.Math;
using System.Collections.Generic;
using System.Linq;

namespace FloorSweep.PathFinding
{
    internal class FDSUpdateMap
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
            var indices = difference.Find(d => d == -1);
            var Ind = new List<Point>(indices);

            foreach (var n in indices)
            {
                foreach (var s in state.Pattern)
                {
                    Ind.Add(n + s);
                }
            }


            var added = Ind.Distinct();

            indices = difference.Find(d => d == 1);

            Ind = new List<Point>(indices);

            foreach (var n in indices)
            {
                foreach (var s in state.Pattern)
                {
                    Ind.Add(n + s);
                }
            }


            var removed = Ind.Distinct();

            foreach (var u in removed)
            {
                foreach (var n in ucc)
                {
                    var s = u + n;
                    Insert(s, H(s, graph), graph, kM, startPos, stack, endPos);
                }
            }
            foreach (var u in added)
            {
                foreach (var n in ucc)
                {
                    var s = u + n;
                    if (T(s, graph) == (double)OutcomeState.CLOSED && H(s, graph) != double.PositiveInfinity)
                    {
                        SetH(s, double.PositiveInfinity, graph);
                        stack.Queue(new Node(s, new PointD(-K(s, graph) + kM, K(s, graph) + G(s, startPos)), endPos));
                        SetT(s, (double)OutcomeState.OPEN, graph);
                    }
                }
            }

            var outState = state;
            outState.Graph = graph;
            outState.Map = map;
            outState.Stack = stack;
            return outState;
        }

        private static double T(Point s, Mat[] graph) => graph[2][s];
        private static double H(Point s, Mat[] graph) => graph[0][s];

        private static double K(Point s, Mat[] graph)
        {
            return graph[1][s];
        }

        private static double G(Point s, Point startPos)
        {
            var ss = (startPos - s).Abs();
            return System.Math.Sqrt(System.Math.Pow(ss.X, 2) + System.Math.Pow(ss.Y, 2));
        }

        private static void SetT(Point s, double val, Mat[] graph)
        {
            graph[2][s.X,s.Y] = val;
        }

        private static void SetH(Point s, double val, Mat[] graph)
        {
            graph[0][s.X,s.Y] = val;
        }

        private static void Insert(Point s, double h_new, Mat[] graph, double kM, Point startPos, PriorityQueue<Node> stack, Point endPos)
        {
            var t = InQ(s, graph);
            if (t == (double)OutcomeState.NEW)
            {
                SetK(s, h_new, graph);
                SetH(s, h_new, graph);
            }
            else
            {
                if (t == (double)OutcomeState.OPEN)
                {
                    SetK(s, System.Math.Min(K(s, graph), h_new), graph);
                }
                else
                {
                    SetK(s, System.Math.Min(H(s, graph), h_new), graph);
                    SetH(s, h_new, graph);
                }

                stack.Queue(new Node(s, new PointD(K(s, graph) + kM + G(s, startPos), K(s, graph) + G(s, startPos)), endPos));
                SetT(s, (double)OutcomeState.OPEN, graph);
            }
        }

        private static double InQ(Point s, Mat[] graph) => graph[2][s];

        private static void SetK(Point s, double val, Mat[] graph)
        {
            graph[1][s.X,s.Y] = val;
        }


    }
}
