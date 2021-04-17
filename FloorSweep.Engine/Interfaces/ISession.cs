using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.Api.Interfaces
{
    public interface ISession
    {
        string Id { get; }
        IPathFindingSession PathFindingSession
        {
            get;
        }
        IRobotStatus CurrentStatus { get; }
    }
}
