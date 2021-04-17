using FloorSweep.Api.Interfaces;
using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ISession = FloorSweep.Api.Interfaces.ISession;

namespace FloorSweep.Api.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IPathFindingAlgorithm _algoritm;
        private readonly Dictionary<string, Session> _sessions = new();
        public SessionRepository(IHttpContextAccessor accessor, IPathFindingAlgorithm algoritm)
        {
            _accessor = accessor;
            _algoritm = algoritm;
        }
        public Task<ISession> GetSessionAsync()
        {
            Session session = GetSessionInternal();
            return Task.FromResult((ISession)session);
        }

        private Session GetSessionInternal()
        {
            var id = _accessor.HttpContext.User.Identity.Name;
            if (!_sessions.ContainsKey(id))
            {
                var mapData = MapData.FromImage("a.png");
                _sessions.Add(id, new Session(id, _algoritm.CreateSession(mapData)));
            }
            var session = _sessions[id];
            return session;
        }

        public Task UpdateStatusAsync(IRobotStatus status)
        {
            var session = GetSessionInternal();
            session.CurrentStatus = status;
            return Task.CompletedTask;
        }
    }
}
