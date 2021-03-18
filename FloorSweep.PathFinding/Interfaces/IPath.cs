using FloorSweep.Math;
using System.Collections.Generic;

namespace FloorSweep.PathFinding.Interfaces
{
    public interface IPath
    {
        IReadOnlyCollection<Point> Path { get; }
#if DEBUG
        ICalculationStatistics CalculationStatistics { get; }
#endif
    }
}
