using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.PathFinding
{
    internal class FocussedDStar : IPathFindingAlgorithm
    {
        
        public IPathFindingSession CreateSession(MapData data) => new FocussedDStarSession(data);
    }
}
