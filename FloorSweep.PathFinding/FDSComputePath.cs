using OpenCvSharp;
using System;
using System.Collections.Generic;
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
            var template = Mat.Zeros(rows: state.Height, state.Width, MatType.CV_64F);
            var outImg = Mat.Zeros(rows: state.Height, state.Width, MatType.CV_64F);
            var kM = state.KM;
            var stack = state.Stack;
            var mindist = double.PositiveInfinity;

            var vis = Mat.Zeros(map.Size(), map.Type());

            //var last = [];
            var found = computeShortestPath(stack, graph, limit, template, ucc, mindist, startPos, pattern, map, kM, vis);
            state.Exist = found;
            if (found)
            {
                seth(startPos, k(startPos, graph), graph);
            }

            state.EndPos = startPos;
            state.StartPos = endPos;
            state.Graph = graph;
            state.Stack = stack;
            var startAcc = startPos.T().ToMat();
            state.Length = graph[1].Get<double>(startAcc.Get<int>(0), startAcc.Get<int>(1));
            return state;
        }


        private static bool computeShortestPath(SortedSet<Mat> stack, Mat[] graph, double limit, Mat template, Mat ucc, double mindist, Mat startPos, Mat pattern, Mat map, double kM, Mat vis)
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
                var x = stack.First();
                stack.Remove(x);
                if (t(x, graph) == OutcomeState.CLOSED)
                {
                    continue;
                }

                var val = x.Col(0).RowRange(2, 3);
                var k_val = k(x, graph);
                var h_val = h(x, graph);
                rsetQ(x, graph);

                if (testNode(x, template, pattern, map))
                {
                    if (h_val > k_val)
                    {
                        foreach (var n in ucc.AsMathlabEnumerable())
                        {
                            var y = (x + n).ToMat();
                            if (t(y, graph) != OutcomeState.NEW && k(x, graph) == k(y, graph) + 1)
                            {
                                seth(x, h(y, graph) + 1, graph);
                                h_val = h(x, graph);
                                setb(x, y, graph);
                                c1 = c1 + 1;
                            }
                        }
                    }
                    if (k_val == h(x, graph) && h(x, graph) < mindist)
                    {
                        c2 = c2 + 1;
                        foreach (var n in ucc.AsMathlabEnumerable())
                        {
                            var y = x + n;
                            if (t(y, graph) == OutcomeState.NEW || h(y, graph) > h_val + 1)
                            {
                                insert(y, h_val + 1, graph, mindist, stack, kM, startPos);
                                setb(y, x, graph);
                            }
                        }
                        var xcol = x.Col(0).RowRange(0, 1);
                        var startPosCol = startPos.Col(0).RowRange(0, 1);
                        if (xcol == startPosCol)
                        {
                            found = true;
                            mindist = Math.Min(mindist, h(x, graph)) + 1;
                            break;
                        }
                    }
                    else if (h_val < mindist || double.IsInfinity(h_val))
                    {
                        foreach (var n in ucc.AsMathlabEnumerable())
                        {
                            var y = x + n;
                            var yAcc = y.T().ToMat();
                            if (t(y, graph) == OutcomeState.NEW || (h(y, graph) != h_val + 1) && vis.Get<int>(yAcc.Get<int>(0), yAcc.Get<int>(1)) != 1)
                            {
                                c3 = c3 + 1;
                                vis.Set<double>(yAcc.Get<int>(0), yAcc.Get<int>(1), 1);
                                insert2(y, h_val + 1, graph, mindist, kM, stack);
                                setb(y, x, graph);
                            }
                            else
                            {
                                if ((b(y, graph) != x) && h(y, graph) > h_val + 1 && t(x, graph) == OutcomeState.CLOSED)
                                {
                                    insert2(x, h_val, graph, mindist, kM, stack);
                                }
                                else if ((b(y, graph) != x) && h_val > h(y, graph) + 1 && t(y, graph) == OutcomeState.CLOSED && cmp(calculateKey(y, kM, graph, startPos), val))
                                {
                                    insert2(y, h(y, graph), graph, mindist, kM, stack);
                                }
                            }
                        }
                    }

                }
                else
                {
                    sett(x, (double)OutcomeState.CLOSED, graph);
                    setk(x, double.PositiveInfinity, graph);
                    seth(x, double.PositiveInfinity, graph);
                }


            }

            Console.WriteLine($"count: {count}");
            Console.WriteLine($" {c1}   {c2}   {c3}");
            return found;
        }

        private static OutcomeState t(Mat s, Mat[] graph)
        {
            var sTrans = s.T().ToMat();
            return (OutcomeState)graph[2].Get<int>(sTrans.Get<int>(0), sTrans.Get<int>(1));
        }

        private static double k(Mat s, Mat[] graph)
        {
            var sAcc = s.T().ToMat();
            return graph[1].Get<double>(sAcc.Get<int>(0), sAcc.Get<int>(1));
        }

        private static void setk(Mat s, double val, Mat[] graph)
        {
            var sAcc = s.T().ToMat();
            graph[1].Set(sAcc.Get<int>(0), sAcc.Get<int>(1), val);
        }

        public static double h(Mat s, Mat[] graph)
        {
            var sAcc = s.T().ToMat();
            return graph[0].Get<double>(sAcc.Get<int>(0), sAcc.Get<int>(1));
        }
        private static void seth(Mat s, double val, Mat[] graph)
        {
            var sAcc = s.T().ToMat();
            graph[0].Set(sAcc.Get<int>(0), sAcc.Get<int>(1), val);
        }

        private static void rsetQ(Mat s, Mat[] graph)
        {
            var sAcc = s.T().ToMat();
            graph[2].Set(sAcc.Get<int>(0), sAcc.Get<int>(1), 0);
        }

        private static bool testNode(Mat s, Mat template, Mat pattern, Mat map)
        {
            var s2 = s.T().ToMat();
            if (template.Get<int>(s2.Get<int>(0), s2.Get<int>(1)) == 1)
            {
                return false;
            }
            foreach (var n in pattern.AsMathlabEnumerable())
            {
                var pos = (s + n).T().ToMat();
                if (map.Get<int>(pos.Get<int>(0), pos.Get<int>(1)) == 0)
                {
                    template.Set(s2.Get<int>(0), s2.Get<int>(1), 1);
                    return false;
                }
            }
            return true;
        }

        private static Mat b(Mat x, Mat[] graph)
        {
            var xAcc = x.T().ToMat();
            var gMat = Mat.FromArray(new double[,] {
                        { graph[5].Get<double>(x.Get<int>(0), x.Get<int>(1))},
                    { graph[6].Get<double>(xAcc.Get<int>(0),xAcc.Get<int>(1))},
                    {0 },
                    {0 }
                    });
            return x + gMat;
        }

        private static OutcomeState inQ(Mat s, Mat[] graph)
        {
            var sAcc = s.T().ToMat();
            return (OutcomeState)graph[2].Get<int>(sAcc.Get<int>(0), sAcc.Get<int>(1));
        }

        private static void setb(Mat x, Mat y, Mat[] graph)
        {
            var val = y.Col(0).RowRange(0, 1) - x.Col(0).RowRange(0, 1);
            var xAcc = x.T().ToMat();
            graph[5].Set(xAcc.Get<int>(0), xAcc.Get<int>(1), val.ToMat().Col(0).Get<double>(0));
            graph[6].Set(xAcc.Get<int>(0), xAcc.Get<int>(1), val.ToMat().Col(0).Get<double>(1));
        }

        private static Mat calculateKey(Mat x, double kM, Mat[] graph, Mat startPos)
        {
            return Mat.FromArray(
                new double[,] {
                        { h(x,graph) + g(x,startPos) + kM },
                        { Math.Min(h(x,graph), k(x,graph)) }
                }
            );
        }

        private static double g(Mat s, Mat startPos)
        {
            var s2 = (startPos - s).Abs().T().ToMat();
            return Math.Sqrt(Math.Pow(s2.Get<double>(0), 2) + Math.Pow(s2.Get<double>(1), 2));
        }

        private static void sett(Mat s, double val, Mat[] graph)
        {
            var s2 = s.T().ToMat();
            graph[2].Set(s2.Get<int>(0), s2.Get<int>(1), val);
        }

        private static void setQ(Mat s, Mat[] graph)
        {
            var s2 = s.T().ToMat();
            graph[2].Set(s2.Get<int>(0), s2.Get<int>(1), 1);
        }

        private static void incr(Mat s, Mat[] graph)
        {
            var s2 = s.T().ToMat();
            graph[3].Set(s2.Get<int>(0), s2.Get<int>(1), graph[3].Get<double>(s2.Get<int>(0), s2.Get<int>(1)) + 1);
        }

        private static Mat calculateKey2(Mat x, Mat[] graph, double kM)
        {
            return Mat.FromArray(new double[,] {
                        { k(x,graph) + kM, Math.Min(h(x,graph), k(x,graph)) }
            });
        }

        public static void insert(Mat s, double h_new, Mat[] graph, double mindist, SortedSet<Mat> stack, double kM, Mat startPos)
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
                    setk(s, Math.Min(k(s, graph), h_new), graph);
                }
                else if (h_new < mindist)
                {

                    setk(s, Math.Min(h(s, graph), h_new), graph);
                }
            }
            seth(s, h_new, graph);
            var key = calculateKey(s, kM, graph, startPos);
            s.Set(2, 0, key.Get<double>(0, 0));
            s.Set(3, 0, key.Get<double>(1, 0));
            stack.Add(s);
            sett(s, (double)OutcomeState.OPEN, graph);

        }


        private static bool cmp(Mat s1, Mat s2)
        {

            var s1Acc = s1.T().ToMat();
            var s2Acc = s2.T().ToMat();
            return s1Acc.Get<double>(0) < s2Acc.Get<double>(0) || (s1Acc.Get<double>(0) == s2.Get<double>(0) && s1Acc.Get<double>(1) < s2Acc.Get<double>(1));
        }
        private static void insert2(Mat s, double h_new, Mat[] graph, double mindist, double kM, SortedSet<Mat> stack)
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
                    setk(s, Math.Min(k(s, graph), h_new), graph);
                }
                else if (h_new < mindist)
                {
                    setk(s, Math.Min(h(s, graph), h_new), graph);
                }
            }
            seth(s, h_new, graph);
            var key2 = calculateKey2(s, graph, kM);
            s.Set(2, 0, key2.Get<double>(0, 0));
            s.Set(3, 0, key2.Get<double>(1, 0));
            stack.Add(s);
            sett(s, (double)OutcomeState.OPEN, graph);

        }
    }
}
