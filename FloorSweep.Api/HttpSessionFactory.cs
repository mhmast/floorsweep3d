using FloorSweep.Engine.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FloorSweep.Api
{
    internal class HttpSessionFactory : ISessionFactory
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ISessionRepository _repo;

        public HttpSessionFactory(IHttpContextAccessor accessor, ISessionRepository repo)
        {
            _accessor = accessor;
            _repo = repo;
        }
        public Task<Engine.Interfaces.ISession> GetSessionAsync()
        {
            var name = _accessor.HttpContext.User.Identity.Name;
            return _repo.EnsureSessionAsync(name);
        }
    }
}
