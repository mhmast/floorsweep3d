using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task SaveObjectAsync<T>(T @object)
        {
            var session = GetSessionInternal();
            session.SetObject(@object);
            return Task.CompletedTask;
        }

        public Task<T> GetObjectAsync<T>() 
        {
            return Task.FromResult(GetSessionInternal().GetObject<T>());
        }

    }
}
