using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class BSpline
    {
        public static Mat[] DoBSpline(Mat points)
        {
            var n = 3;
            var len = points.Rows;
            var m = len - n;
            var f = Mat.Zeros(m, 4).ToMat();
            var g = Mat.Zeros(m, 4).ToMat();
            var e = Mat.Zeros(m + 1, 4).ToMat();
           
            for (var i = 0; i < f.Cols; i++)
            {
                f.Set(0, i, points.Get<double>(1, i));
            }
            for (var i = 0; i < g.Cols; i++)
            {
                g.Set(0, i, points.Get<double>(1, i) + points.Get<double>(2, i)/2);
            }

            for (int i = 1; i < m - 2; i++) {
                for (var j = 0; j < f.Cols; j++)
                {
                    f.Set(i, j, 2 * (points.Get<double>(i + 1, j) + points.Get<double>(i + 2, j)) / 3);
                }
                for (var j = 0; j < g.Cols; j++)
                {
                    g.Set(i, j, (points.Get<double>(i + 1, j) + 2 * points.Get<double>(i + 2, j)) / 3);
                }
            }

            for (var j = 0; j < f.Cols; j++)
            {
                f.Set(m, j, (points.Get<double>(m + 2,j) + points.Get<double>(m + 1,j) )/ 2);
            }
            for (var j = 0; j < g.Cols; j++)
            {
                g.Set(m-1, j, points.Get<double>(m + 1, j));
            }
            for (var j = 0; j < e.Cols; j++)
            {
                e.Set(0, j, points.Get<double>(0, j));
            }

            for (int i = 1; i < m; i++) {
                for (var j = 0; j < e.Cols; j++)
                {
                    e.Set(0, j, (g.Get<double>(i - 1,j) + f.Get<double>(i,j)) / 2);
                }
            }

            for (var j = 0; j < e.Cols; j++)
            {
                e.Set(m, j, points.Get<double>(m + 2,j));
            }

            Mat[] retVal = new Mat[m];

            for (int i = 0; i < m; i++) {

                var m1 = new Mat();
                m1.Add(e.Row(i));
                m1.Add(f.Row(i));
                m1.Add(g.Row(i));
                m1.Add(e.Row(i+1));
                retVal[i] = m1;
            }
            var spline = new Mat();
            t = 0:0.2:1;

            for i = 1:1:length(BSpline)


                spline = [spline; bezier4(BSpline{ i}, t, 3)];
            end
    
    out = Interpolate(round(spline), 1, 'linear');


            end




}
    }
