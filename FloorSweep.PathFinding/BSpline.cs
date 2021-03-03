using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class BSpline
    {
        public static Mat DoBSpline(Mat points)
        {
            var n = 3;
            var len = points.Rows;
            var m = len - n;
            var f = Mat.Zeros(MatType.CV_64FC1, m, 4).ToMat();
            var g = Mat.Zeros(MatType.CV_64FC1, m, 4).ToMat();
            var e = Mat.Zeros(MatType.CV_64FC1, m + 1, 4).ToMat();

            for (var i = 1; i <= f.Cols; i++)
            {
                f._Set<double>(1, i, points._<double>(1, i));
            }
            for (var i = 1; i <= g.Cols; i++)
            {
                g._Set<double>(1, i, points._<double>(1, i) + points._<double>(2, i) / 2);
            }

            for (int i = 1; i < m - 2; i++)
            {
                for (var j = 1; j <= f.Cols; j++)
                {
                    f._Set<double>(i, j, 2 * (points._<double>(i + 1, j) + points._<double>(i + 2, j)) / 3);
                }
                for (var j = 1; j <= g.Cols; j++)
                {
                    g._Set<double>(i, j, (points._<double>(i + 1, j) + 2 * points._<double>(i + 2, j)) / 3);
                }
            }

            for (var j = 1; j <= f.Cols; j++)
            {
                f._Set<double>(m, j, (points._<double>(m + 1, j) + points._<double>(m, j)) / 2);
            }
            for (var j = 1; j <= g.Cols; j++)
            {
                g._Set<double>(m, j, points._<double>(m + 1, j));
            }
            for (var j = 1; j <= e.Cols; j++)
            {
                e._Set<double>(1, j, points._<double>(1, j));
            }

            for (int i = 2; i <= m; i++)
            {
                for (var j = 1; j <= e.Cols; j++)
                {
                    e._Set<double>(1, j, (g._<double>(i - 1, j) + f._<double>(i, j)) / 2);
                }
            }

            for (var j = 1; j <= e.Cols; j++)
            {
                e._Set<double>(m, j, points._<double>(m + 2, j));
            }

            Mat[] retVal = new Mat[m];

            for (int i = 1; i <= m; i++)
            {

                var m1 = new Mat(rows: 0, e.Cols, e.Type());
                m1.AddBottom(e.Rows(i, i));
                m1.AddBottom(f.Rows(i, i));
                m1.AddBottom(g.Rows(i, i));
                m1.AddBottom(e.Rows(i + 1, i + 1));
                retVal[i - 1] = m1;
            }
            var spline = new Mat(rows: 0, e.Cols, MatType.CV_64FC1);
            var t = MatExtensions.FromRange(0, 1, 0.2);

            foreach (var i in retVal)
            {
                spline.AddBottom(bezier4(i, t, 3));
            }

            return Interpolate(spline.Round(), 1);


        }

        private static double w_b(double t, double n, double i)
        {
            if (i < 0 || i > n)
            {
                return -1;
            }
            else
            {

                return nchoosek(n, i) * Math.Pow(t, i) * Math.Pow(1 - t, n - i);
            }
        }

        private static Mat Interpolate(Mat points, double sampling = 1)
        {

            var indicesToDelete = new List<int>();
            for (int i = 1; i <= points.Rows - 1; i++)
            {
                if (w_distance(points.Rows(i, i), points.Rows(i + 1, i + 1)) < 0.01)
                {
                    indicesToDelete.Add(i);
                }
            }

            points = points.RemoveRows(indicesToDelete.ToArray());

            if (points.Rows < 4)
            {
                return points;
            }

            var len = points.Rows;
            var distance = Mat.Zeros(rows: len, 1, MatType.CV_64FC1).ToMat();

            for (int i = 2; i <= len; i++)
            {
                var delta = w_distance(points.Rows(i, i), points.Rows(i - 1, i - 1));
                var val = delta + distance._<double>(i - 1);
                if(double.IsNaN(val))
                {
                    //Debugger.Break();
                }
                distance._Set<double>(i, val);
            }

            var t = MatExtensions.FromRange(0, distance.__(distance.Cols), sampling);
            return interp1(distance, points, t);
        }

        private static double w_distance(Mat p1, Mat p2)
        {
            var vec = p1.Minus(p2);
            return Math.Sqrt(vec.Pow(2).Sum2());
        }

        private static Mat interp1(Mat X, Mat Y, Mat targetX)
        {
            Mat retVal = new Mat(rows: 0, X.Cols, MatType.CV_64FC1);
            for (int i = 1; i <= X.Cols; i++)
            {
                retVal.AddBottom(interp1(X, Y, targetX.__(i)));
            }
            return retVal.T().ToMat();
        }
        private static double interp1(Mat X, Mat Y, int targetX)
        {
            var dist = X.Minus(targetX).Abs().ToMat();
            double minVal, maxVal;
            Point minLoc1, minLoc2, maxLoc;

            // find the nearest neighbour
            Mat mask = Mat.Ones(X.Rows, X.Cols, MatType.CV_8UC1);
            Cv2.MinMaxLoc(dist, out minVal, out maxVal, out minLoc1, out maxLoc, mask);
            minLoc1 = minLoc1 + new Point(1, 1);
            // mask out the nearest neighbour and search for the second nearest neighbour
            mask._Set<byte>(minLoc1.Y, minLoc1.X,  0);
            Cv2.MinMaxLoc(dist, out minVal, out maxVal, out minLoc2, out maxLoc, mask);
            minLoc2 = minLoc2 + new Point(1, 1);

            // use the two nearest neighbours to interpolate the target value
            double res = interpolate(X.__(minLoc1.Y, minLoc1.X), Y._<double>(minLoc1.Y, minLoc1.X), X.__(minLoc2.Y, minLoc2.X), Y._<double>(minLoc2.Y, minLoc2.X), targetX);
            return res;
        }

        private static double interpolate(int x1, double y1, int x2, double y2, int targetX)
        {
            int diffX = x2 - x1;
            double diffY = y2 - y1;
            int diffTarget = targetX - x1;

            return y1 + (diffTarget * diffY) / diffX;
        }

        private static double nchoosek(double N, double K)
        {
            var result = 1.0;
            for (var i = 1; i <= K; i++)
            {
                result *= N - (K - i);
                result /= i;
            }
            return result;
        }

        private static Mat bezier4(Mat bez, Mat t, double n)
        {
            var len = Math.Max(t.Rows, t.Cols);
            var @out = Mat.Zeros(rows: 0, bez.Cols, MatType.CV_64FC1).ToMat();
            for (int i = 1; i <= len; i++)
            {
                var k = bez._<double>(1, bez.Cols) * (w_b(t._<double>(i), n, 0));
                var l = bez._<double>(2, bez.Cols) * (w_b(t._<double>(i), n, 1));
                var m = bez._<double>(3, bez.Cols) * (w_b(t._<double>(i), n, 2));
                var u = bez._<double>(4, bez.Cols) * (w_b(t._<double>(i), n, 3));
                var newrow = bez.Rows(1, 1).Mult(k).Plus(bez.Rows(2, 2).Mult(l)).Plus(bez.Rows(3, 3).Mult(m)).Plus(bez.Rows(4, 4).Mult(u)).Div(k + l + m + u);
                if(newrow.DataLeftToRight<double>().Any(double.IsNaN))
                {
                    //Debugger.Break();
                }
                @out.AddBottom(newrow);
            }
            return @out;
        }
    }
}
