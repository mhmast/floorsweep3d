namespace FloorSweep.Math
{
    public struct PointD
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
