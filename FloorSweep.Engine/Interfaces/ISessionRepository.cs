using System.Threading.Tasks;

namespace FloorSweep.Api.Interfaces
{
    public interface ISessionRepository
    {
        Task<ISession> GetSessionAsync();
        Task UpdateStatusAsync(IRobotStatus status);
    }
}
