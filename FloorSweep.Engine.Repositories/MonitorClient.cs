using FloorSweep.Engine.Interfaces;
using System.Net;

namespace FloorSweep.Engine.Repositories
{
    internal class MonitorClient : IMonitorClient
    {
        public MonitorClient(ISession session, IPAddress clientIp)
        {
            SessionId = session.Id;
            ClientIP = clientIp;
        }

        public string SessionId { get; }

        public IPAddress ClientIP { get; }
    }
}