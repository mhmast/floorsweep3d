using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IMonitorRepository
    {
        Task AddMonitorClientAsync(ISession session,IPAddress clientIP);
        Task<IEnumerable<IMonitorClient>> GetMonitorClientsAsync(ISession session);
    }
}
