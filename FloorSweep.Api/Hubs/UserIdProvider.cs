using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace FloorSweep.Api.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserIdProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string GetUserId(HubConnectionContext connection)
        => connection.User.Identity.Name;

        public string GetUserId()
        => _contextAccessor.HttpContext.User.Identity.Name;
    }
}
