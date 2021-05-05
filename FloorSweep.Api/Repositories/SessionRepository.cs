using System.Collections.Generic;
using System.Threading.Tasks;
using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.Session;
using Microsoft.AspNetCore.Http;

namespace FloorSweep.Api.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly Dictionary<string, Session> _sessions = new();
        public SessionRepository(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<Engine.Session.ISession> GetSessionAsync()
        {
            Session session = GetSessionInternal();
            return Task.FromResult((Engine.Session.ISession)session);
        }

        private Session GetSessionInternal()
        {
            var id = _accessor.HttpContext.User.Identity.Name;
            if (!_sessions.ContainsKey(id))
            {
               // var mapData = MapData.FromImage("a.png");
                _sessions.Add(id, new Session(id));
            }
            var session = _sessions[id];
            return session;
        }

        public Task<bool> SaveObjectAsync<T>(T @event)
        {
            var session = GetSessionInternal();
            session.RobotStatus = status;
            return Task.FromResult(false);
        }

        public Task<bool> OnStatusUpdatedAsync(ILocationStatus status)
        {
            var session = GetSessionInternal();
            session.LocationStatus = status;
            return Task.FromResult(false);
        }
    }
}
