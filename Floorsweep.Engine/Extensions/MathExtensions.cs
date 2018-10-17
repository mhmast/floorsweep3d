using System;

namespace FloorSweep.Engine.Extensions
{
    public static class MathExtensions
    {
        public static int RoundToNearestMultipleOf(this int number, int factor)
        {
            return (int) Math.Round(number / (double) factor, MidpointRounding.AwayFromZero) * factor;
        }
    }
}
