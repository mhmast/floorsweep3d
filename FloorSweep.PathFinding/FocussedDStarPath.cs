using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class FocussedDStarPath : IPath
    {
        public FocussedDStarPath(IReadOnlyCollection<Point> result
#if DEBUG
            ,ICalculationStatistics statistics)
            #else
            )
#endif
        {
            Path = result;
            CalculationStatistics = statistics;
        }
        public IReadOnlyCollection<Point> Path { get; }

#if DEBUG
        public ICalculationStatistics CalculationStatistics { get; }
#endif
    }
}
