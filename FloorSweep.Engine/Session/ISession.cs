using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.Engine.Session
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
