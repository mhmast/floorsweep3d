using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.Api.Repositories
{
    internal class Session : ISession
    {
        public string Id { get; }

        public Session(string id)
        {
            Id = id;
        }

        public IPathFindingSession PathFindingSession
        {
            get; set;
        }

        public IRobotStatus RobotStatus { get; set; }

        public ILocationStatus LocationStatus { get; set; }
    }
}
