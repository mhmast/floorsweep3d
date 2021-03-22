using System.Net;

namespace FloorSweep.Engine.Interfaces
{
    public interface  IMonitorClient
    {
        string SessionId { get; }
        IPAddress ClientIP { get; }


    }
}
