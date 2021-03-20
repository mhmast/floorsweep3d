using FloorSweep.Engine.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private Dictionary<string, ISession> _sessions = new Dictionary<string, ISession>(); 

        public Task<ISession> EnsureSessionAsync(string id)
        {

            if (!_sessions.ContainsKey(id))
            {
                var session = new Session(id);
                _sessions.Add(session.Id, session);

            }
            return Task.FromResult(_sessions[id]);
        }

    }
}
