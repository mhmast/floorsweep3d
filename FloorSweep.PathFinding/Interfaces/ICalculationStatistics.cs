using System.Collections.Generic;

namespace FloorSweep.PathFinding.Interfaces
{
    public interface ICalculationStatistics : IDictionary<string,long>
    {
        public long Total { get; }
    }
}
