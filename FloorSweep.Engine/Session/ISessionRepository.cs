using FloorSweep.Engine.Map;
using FloorSweep.Engine.Models;
using FloorSweep.Engine.StatusHandlers;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Session
{
    public interface ISessionRepository : IStatusUpdateHandler<IRobotStatus>, IStatusUpdateHandler<ILocationStatus>
    {
        Task<ISession> GetSessionAsync();
    }
}
