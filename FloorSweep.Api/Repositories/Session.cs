using FloorSweep.Api.Interfaces;
using FloorSweep.Engine.Interfaces;
using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;

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
