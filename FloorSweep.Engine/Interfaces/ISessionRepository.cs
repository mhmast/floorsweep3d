using FloorSweep.Engine.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Api.Interfaces
{
    public interface ISessionRepository : IStatusUpdateHandler<IRobotStatus>, IStatusUpdateHandler<ILocationStatus>
    {
        Task<ISession> GetSessionAsync();
    }
}
