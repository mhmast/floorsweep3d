using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IMonitorService
    {
        Task RegisterMonitorAsync(ISession session, string clientIP);
    }
}