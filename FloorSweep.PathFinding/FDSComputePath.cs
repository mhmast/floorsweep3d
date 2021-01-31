using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class FDSComputePath
    {
        enum OutcomeState
        {
            NEW = -1,
            OPEN = 1,
            CLOSED = 0
        }
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
            var SQRT2 = Math.Sqrt(2) - 1;
            var stack = state.Stack;
            var mindist = double.PositiveInfinity;

            var vis = Mat.Zeros(map.Size(), map.Type());

            //var last = [];
            var found = false;
            computeShortestPath(stack, graph, limit, template, ucc, mindist);
            state.exist = found;
            if found
                fprintf('znaleziono rozwi�zanie!: %d\n', graph(startPos(1), startPos(2), 2));
            seth(startPos, k(startPos));
            end

            outState = state;
            outState.goal = startPos;
            outState.start = endPos;
            outState.graph = graph;
            outState.stack = stack;
            outState.length = graph(startPos(1), startPos(2), 2);

        }


        private void computeShortestPath(SortedSet<Mat> stack, Mat[] graph, double limit, MatExpr template, Mat ucc, double mindist)
        {
            var terminate = false;
            var count = 0;
            var c1 = 0;
            var c2 = 0;
            var c3 = 0;

            while (!terminate && stack.Count > 0)
            {

                if (count > limit)
                {
                    terminate = true;
                }
                count = count + 1;
                var x = stack.GetEnumerator().Current;
                stack.Remove(x);
                if (t(x, graph) == OutcomeState.CLOSED)
                {
                    continue;
                }

                var val = x(3:4);
                var k_val = k(x, graph);
                var h_val = h(x, graph);
                rsetQ(x, graph);

                if (testNode(x, template))
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
                                insert(y, h_val + 1);
                                setb(y, x)
                                }
                        }
                        if isequal(x(1:2), startPos(1:2))
                            found = true;
                        mindist = min(mindist, h(x)) + 1;
                        break
                        end
        elseif h_val < mindist || isinf(h_val) % propagate RAISE state
                        for n = ucc
                                y = x + n;
                                if t(y) == NEW || (h(y) ~= h_val + 1) && vis(y(1), y(2))~= 1
                                    c3 = c3 + 1;
                        vis(y(1), y(2)) = 1;
                        insert2(y, h_val + 1);
                        setb(y, x);
                                else
                                    if ~isequal(b(y), x) && h(y) > h_val + 1 && t(x) == CLOSED
                                        insert2(x, h_val);
                        elseif ~isequal(b(y), x) && h_val > h(y) + 1 && t(y) == CLOSED && cmp(calculateKey(y), val)
                                        insert2(y, h(y));
                        end
                    end
                        end
        end
    else
                            sett(x, CLOSED);
                        setk(x, inf)
        seth(x, inf)
    end


    end
        fprintf('count: %d\n', count);
                        fprintf(' %d   %d   %d\n', c1, c2, c3);
                        end
    }

                    private OutcomeState t(Mat s, Mat[] graph)
                    {
                        var sTrans = s.T().ToMat();
                        return (OutcomeState)graph[2].Get<int>(sTrans.Get<int>(0), sTrans.Get<int>(1));
                    }

                    private double k(Mat s, Mat[] graph)
                    {
                        var sAcc = s.T().ToMat();
                        return graph[1].Get<double>(sAcc.Get<int>(0), sAcc.Get<int>(1));
                    }

                    private void setk(Mat s, double val, Mat[] graph)
                    {
                        var sAcc = s.T().ToMat();
                        graph[1].Set(sAcc.Get<int>(0), sAcc.Get<int>(1), val);
                    }

                    private double h(Mat s, Mat[] graph)
                    {
                        var sAcc = s.T().ToMat();
                        return graph[0].Get<double>(sAcc.Get<int>(0), sAcc.Get<int>(1));
                    }
                    private void seth(Mat s, double val, Mat[] graph)
                    {
                        var sAcc = s.T().ToMat();
                        graph[0].Set(sAcc.Get<int>(0), sAcc.Get<int>(1), val);
                    }

                    private void rsetQ(Mat s, Mat[] graph)
                    {
                        var sAcc = s.T().ToMat();
                        graph[2].Set(sAcc.Get<int>(0), sAcc.Get<int>(1), 0);
                    }

                    private bool testNode(Mat s, Mat template)
                    {
                        var sAcc = s.T().ToMat();
                        return template.Get<int>(sAcc.Get<int>(0), sAcc.Get<int>(1)) == 1;
                    }

                    private Mat b(Mat x, Mat[] graph)
                    {
                        var xAcc = x.T().ToMat();
                        var gMat = Mat.FromArray(new double[,] {
                        { graph[5].Get<double>(x.Get<int>(0), x.Get<int>(1))},
                    { graph[6].Get<double>(xAcc.Get<int>(0),xAcc.Get<int>(1)},
                    {0 },
                    {0 }
                    });
                        return x + gMat;
                    }

                    private OutcomeState inQ(Mat s, Mat[] graph)
                    {
                        var sAcc = s.T().ToMat();
                        return (OutcomeState)graph[2].Get<int>(sAcc.Get<int>(0), sAcc.Get<int>(1));
                    }

                    private void setb(Mat x, Mat y, Mat[] graph)
                    {
                        var val = y.Col(0).RowRange(0, 1) - x.Col(0).RowRange(0, 1);
                        var xAcc = x.T().ToMat();
                        graph[5].Set(xAcc.Get<int>(0), xAcc.Get<int>(1), val.ToMat().Col(0).Get<double>(0));
                        graph[6].Set(xAcc.Get<int>(0), xAcc.Get<int>(1), val.ToMat().Col(0).Get<double>(1));
                    }

                    private Mat calculateKey(Mat x, double kM, Mat[] graph)
                    {
                        return Mat.FromArray(new double[,] {
          { h(x,graph) + g(x,graph) + kM },
                        { Math.Min(h(x,graph), k(x,graph)) }
        });
                    }

                    private double g(Mat s,Mat startPos) {
                        var s2= (startPos - s).ToMat().Abs();

        out = sqrt(s(1) ^ 2 + s(2) ^ 2);
                    }


                    private void insert(Mat s, double h_new, Mat[] graph, double mindist)
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
                        s(3:4) = calculateKey(s);
                    add(stack, s);
                    sett(s, OPEN);

                    end





            function sett(s, val)
    graph(s(1), s(2), 3) = val;
                    end
                    function setQ(s)
     graph(s(1), s(2), 3) = 1;
                    end

                    function incr(s)
    graph(s(1), s(2), 4) = graph(s(1), s(2), 4) + 1;
                    end

                    % -----------------------------------------------------------
                    

                    % -----------------------------------------------------------
        

                    % -----------------------------------------------------------

                    end
            function out = calculateKey2(x)
    out =  [
           k(x) + kM
          min(h(x), k(x))
        ];
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
function out = cmp(s1, s2) % s1 > s2
    out = s1(1) < s2(1) || (s1(1) == s2(1) && s1(2) < s2(2));
                    end
                    % -----------------------------------------------------------

                    function insert2(s, h_new)
                            % aktualizuje g(s), iweijgrijgiiEGWNAGRKEPORORPO
                            t = inQ(s);
                    if t == NEW && h_new < mindist
                        setk(s, h_new);
                    else
                        if t == OPEN && h_new < mindist
                            setk(s, min(k(s), h_new));
                    elseif h_new<mindist
        
                        setk(s, min(h(s), h_new) );
                    end
                end
        seth(s, h_new);
                    s(3:4) = calculateKey2(s);
                    add(stack, s);
                    sett(s, OPEN);

                    end
                    % -----------------------------------------------------------


        }
            }
        }
