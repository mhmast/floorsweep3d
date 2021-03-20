using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface ISessionRepository
    {
        Task<ISession> EnsureSessionAsync(string id);
    }
}
