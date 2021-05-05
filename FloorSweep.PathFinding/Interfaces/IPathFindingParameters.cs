using FloorSweep.Math;

namespace FloorSweep.PathFinding.Interfaces
{
    public interface IPathFindingParameters
    {
        Point Start { get; }
        Point Target { get; }
    }
}
