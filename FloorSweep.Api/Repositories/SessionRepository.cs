using System.Collections.Generic;
using System.Threading.Tasks;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Session;
using Microsoft.AspNetCore.Http;

namespace FloorSweep.Api.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IEventService _eventService;
        private readonly Dictionary<string, Session> _sessions = new();
        public SessionRepository(IHttpContextAccessor accessor, IEventService eventService)
        {
            _accessor = accessor;
            _eventService = eventService;
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

        public async Task SaveObjectAsync<T>(T @object)
        {
            var session = GetSessionInternal();
            session.SetObject(@object);
            await _eventService.SendSessionUpdatedAsync(this);
            return Task.CompletedTask;
        }

        public Task<T> GetObjectAsync<T>() 
        {
            return Task.FromResult(GetSessionInternal().GetObject<T>());
        }

    }
}
