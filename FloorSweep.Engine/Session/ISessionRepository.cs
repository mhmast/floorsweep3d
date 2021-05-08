using System.Threading.Tasks;

namespace FloorSweep.Engine.Session
{
    public interface ISessionRepository
    {
        Task<T> GetObjectAsync<T>(string key) ;
        Task SaveObjectAsync<T>(T @object) where T : IKeyable;
        Task SaveObjectAsync(string key,object @object) ;
        Task<ISession> GetSessionAsync();
    }
}
