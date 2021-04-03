using FloorSweep.Engine.Interfaces;
using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IPathFindingAlgorithm _algoritm;
        private Dictionary<string, ISession> _sessions = new Dictionary<string, ISession>();
        public SessionRepository(IPathFindingAlgorithm algoritm)
        {
            _algoritm = algoritm;
        }
        public Task<ISession> EnsureSessionAsync(string id)
        {

            if (!_sessions.ContainsKey(id))
            {
                var mapData = MapData.FromImage("a.png");
                var session = new Session(id,_algoritm.CreateSession(mapData));
                _sessions.Add(session.Id, session);

            }
            return Task.FromResult(_sessions[id]);
        }

    }
}
