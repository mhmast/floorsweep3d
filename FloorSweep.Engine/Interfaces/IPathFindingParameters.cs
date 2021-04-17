using FloorSweep.Math;

namespace FloorSweep.Api.Interfaces
{
    public interface IPathFindingParameters
    {
        Point Start { get; }
        Point Target { get; }
    }
}
