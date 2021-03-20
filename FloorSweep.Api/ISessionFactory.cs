using System.Threading.Tasks;
using ISession = FloorSweep.Engine.Interfaces.ISession;

namespace FloorSweep.Api
{
    public interface ISessionFactory
    {
        Task<ISession> GetSessionAsync();
    }
}
