using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class FDSInit
    {
        public static void DoFDSInit(Mat startPos, Mat endPos, Mat map, int scalling)
        {
            var radius = 10 / scalling;
            var mat = Mat.Zeros(radius * 2, radius * 2).ToMat();
            double dista(double a, double b) => Math.Sqrt(a * a + b * b);

            var shapePattern = Mat.Zeros(rows: 0, 4, map.Type()).ToMat();

            for (int x = 0; x < 2 * radius; x++)
            {
                for (int y = 0; y < 2 * radius; y++)
                {
                    if (dista(x - radius - 0.5, y - radius - 0.5) > radius - 1 && dista(x - radius - 0.5, y - radius - 0.5) < radius)
                    {
                        mat.Set(x, y, 1);
                        shapePattern.Add(new Vec4i(x - radius, y - radius, 0, 0));
                    }
                }
            }
            var neighbours = Mat.FromArray(new[,] {
                { 0, 1, 0, 0, },
            { -1, 0, 0, 0 },
            { 0, - 1, 0, 0 },
            { 1, 1, 0, 0 },
            { -1, - 1, 0, 0 },
            { 1, 0, 0, 0 },
            { 1, - 1, 0, 0 },
            { -1, 1, 0, 0 }
            }).ComplexConjugate();

            var @out = new State();

            @out.Map = map;
            @out.StartPos = startPos.Col(1).RowRange(0, 1).Inv().ToMat();
            @out.StartPos.Add(Mat.Zeros(rows: 2, startPos.Cols, startPos.Type()));
            @out.EndPos = endPos.Col(1).RowRange(0, 1).Inv().ToMat();
            @out.EndPos.Add(Mat.Zeros(rows: 2, endPos.Cols, endPos.Type()));
            startPos = @out.StartPos;
    
    @out.Scaling = scalling;
    @out.Pattern = shapePattern.ComplexConjugate();
    @out.Ucc = neighbours;
    @out.Height = @out.Map.Rows;
    @out.Width = @out.Map.Cols;
    out.graph = zeros(out.height, out.width, 7);
    out.graph(:,:, 1:2) = inf;
    out.graph(:,:, 3) = -1;
    out.graph(:,:, 5) = -1;
    out.graph(:,:, 6) = 0;
    out.graph(:,:, 7) = 0;
    out.kM = 0;
            SQRT2 = sqrt(2) - 1;
    out.comparator = DStarcmp;
    out.stack = java.util.PriorityQueue(180247, out.comparator);

            setk(out.endPos, 0);
            seth(out.endPos, 0);
            setQ(out.endPos);
    out.endPos(3:4) = [heur(out.endPos); 0];
            add(out.stack, out.endPos);



    % -----------------------------------------------------------
    function setQ(s)
         out.graph(s(1), s(2), 3) = 1;
            end
            function seth(s, val)
        out.graph(s(1), s(2), 1) = val;
            end
            function setk(s, val)
        out.graph(s(1), s(2), 2) = val;
            end
            function out = heur(s)
        k = abs(startPos - s);
        out = SQRT2 * min(k(1:2)) + max(k(1:2));
            end



        end
    }
    }
