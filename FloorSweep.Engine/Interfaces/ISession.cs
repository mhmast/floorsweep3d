using FloorSweep.Engine.Interfaces;
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
        IRobotStatus RobotStatus { get; }

        ILocationStatus LocationStatus { get; }
    }
}
