namespace FloorSweep.Math
{
    public struct PointD
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static PointD Up => new PointD(0, 1);
        public double X { get; }
        public double Y { get; }

        public static PointD operator +(PointD left, PointD right) => new PointD(left.X + right.X, left.Y + right.Y);
        public static PointD operator +(PointD left, int right) => new PointD(left.X + right, left.Y + right);
        public static PointD operator *(PointD left, Mat right) => right.Mul(left);
        public static PointD operator *(PointD left, int right) => new PointD(left.X * right, left.Y * right);
        public static PointD operator -(PointD left, PointD right) => new PointD(left.X - right.X, left.Y - right.Y);

        public static implicit operator Point(PointD p) => new Point((int)p.X, (int)p.Y);
    }
}
