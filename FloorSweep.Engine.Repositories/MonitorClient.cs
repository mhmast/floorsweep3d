using FloorSweep.Engine.Interfaces;

namespace FloorSweep.Engine.Repositories
{
    internal class MonitorClient : IMonitorClient
    {
        public MonitorClient(ISession session, string clientIp)
        {
            SessionId = session.Id;
            ClientIP = clientIp;
        }

        public string SessionId { get; }

        public string ClientIP { get; }
    }
}