using FloorSweep.Api.Interfaces;
using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Api.Repositories
{
    internal class Session : ISession
    {
        public string Id { get; }

        public Session(string id, IPathFindingSession session)
        {
            Id = id;
            PathFindingSession = session;
        }

        public IPathFindingSession PathFindingSession
        {
            get;
        }

        public IRobotStatus CurrentStatus { get; set; }

    }
}
