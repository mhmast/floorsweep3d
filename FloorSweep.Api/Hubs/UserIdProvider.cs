using Microsoft.AspNetCore.SignalR;

namespace FloorSweep.Api.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {

        public string GetUserId(HubConnectionContext connection)
        =>
            connection.User.Identity.Name;

    }
}
