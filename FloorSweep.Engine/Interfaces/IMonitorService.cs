using System.Net;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IMonitorService
    {
        Task RegisterMonitorAsync(ISession session, IPAddress clientIP);
    }
}