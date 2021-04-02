using FloorSweep.Engine.Interfaces;
using FloorSweep.PathFinding.Interfaces;

namespace FloorSweep.Engine.Repositories
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
    }
}
