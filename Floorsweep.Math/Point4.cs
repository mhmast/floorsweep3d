using System;
using System.Collections.Generic;
using System.Drawing;

namespace FloorSweep.Math
{
    public class Point4
    {
      
        public Point4(Point xy, PointD ab,Point targetPos)
        {
            XY = xy;
            AB = ab;
            Length = System.Math.Abs(xy.Length - targetPos.Length);
        }

        public Point XY { get; set; }
        public PointD AB { get; set; }
        public long Length { get; }

    }

    public class Point
    {
        private static IEqualityComparer<Point> _comparer = new PointComparer();

        class PointComparer : IEqualityComparer<Point>
        {
            public bool Equals(Point x, Point y)
            => x == y;

            public int GetHashCode(Point obj)
            {
                return obj.GetHashCode();
            }
        }


        public static IEqualityComparer<Point> Comparer
        {
            get => _comparer;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
            Length = X + Y;
        }

        public int Min() => X < Y ? X : Y;
        public int Max() => X > Y ? X : Y;
        public int X { get; }
        public int Y { get; }
        public int Length { get;  }

        public static bool operator ==(Point left, Point right) => left?.X == right?.X && left?.Y == right?.Y;
        public static bool operator !=(Point left, Point right) => !(left == right);
        public static Point operator +(Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);
        public static Point operator +(Point left, int right) => new Point(left.X + right, left.Y + right);
        public static Point operator -(Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);
        public static Point operator -(Point left, int right) => new Point(left.X - right, left.Y - right);
        public static Point operator /(Point left, Point right) => new Point(left.X / right.X, left.Y / right.Y);
        public static Point operator /(Point left, int right) => new Point(left.X / right, left.Y / right);
        public static Point operator *(Point left, Point right) => new Point(left.X * right.X, left.Y * right.Y);
        public static Point operator *(Point left, int right) => new Point(left.X * right, left.Y * right);

        public Point Abs() => new Point(System.Math.Abs(X), System.Math.Abs(Y));

        public static implicit operator PointF(Point p) => new PointF(p.X, p.Y);

        public long Sum()
        => X + Y;
    }

    public class PointD
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; }
        public double Y { get; }

        public static PointD operator +(PointD left, PointD right) => new PointD(left.X + right.X, left.Y + right.Y);
        public static PointD operator -(PointD left, PointD right) => new PointD(left.X - right.X, left.Y - right.Y);
    }
}
