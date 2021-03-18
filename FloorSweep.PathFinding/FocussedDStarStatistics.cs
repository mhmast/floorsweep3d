using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class FocussedDStarStatistics : Dictionary<string, long>, ICalculationStatistics
    {
        private readonly string _totalKey;

        public FocussedDStarStatistics(string totalKey)
        {
            _totalKey = totalKey;
        }
        public long Total => this[_totalKey];
    }
}
