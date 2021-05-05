using System.Threading.Tasks;

namespace FloorSweep.Engine.Session
{
    public interface ISessionRepository 
    {
        Task<bool> SaveObjectAsync<T>(T status);

        Task<ISession> GetSessionAsync();
    }
}
