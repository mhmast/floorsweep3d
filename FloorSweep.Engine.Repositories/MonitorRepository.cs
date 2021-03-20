using FloorSweep.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Repositories
{
    internal class MonitorRepository : IMonitorRepository
    {
        private Dictionary<string, List<IMonitorClient>> _clients = new Dictionary<string, List<IMonitorClient>>();

        public Task AddMonitorClientAsync(ISession session,string clientIP)
        {
            if(!_clients.ContainsKey(session.Id))
            {
                _clients.Add(session.Id, new List<IMonitorClient>());
            }
            if(!_clients[session.Id].Any(c=>c.ClientIP == clientIP))
            {
                _clients[session.Id].Add(new MonitorClient(session,clientIP));
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IMonitorClient>> GetMonitorClientsAsync(ISession session) => Task.FromResult(_clients.ContainsKey(session.Id) ? _clients[session.Id] : Enumerable.Empty<IMonitorClient>());
    }
}
