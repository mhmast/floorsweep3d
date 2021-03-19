using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class FocussedDStarPath : IPath
    {
        public FocussedDStarPath(IReadOnlyCollection<Point> result
            , ICalculationStatistics statistics)
        {
            Path = result;
            CalculationStatistics = statistics;
        }
        public IReadOnlyCollection<Point> Path { get; }

        public ICalculationStatistics CalculationStatistics { get; }

    }
}
