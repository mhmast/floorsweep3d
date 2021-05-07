using System.Threading.Tasks;

namespace FloorSweep.Engine.Session
{
    public interface ISessionRepository
    {
        Task<T> GetObjectAsync<T>();
        Task SaveObjectAsync<T>(T @object);
    }
}
