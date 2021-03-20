using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IMonitorRepository
    {
        Task AddMonitorClientAsync(ISession session,string clientIP);
        Task<IEnumerable<IMonitorClient>> GetMonitorClientsAsync(ISession session);
    }
}
