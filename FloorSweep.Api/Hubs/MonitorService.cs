﻿using FloorSweep.Api.Hubs.Dtos;
using FloorSweep.Engine.Interfaces;
using FloorSweep.Math;
using FloorSweep.PathFinding.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FloorSweep.Api.Hubs
{
    [Authorize]
    [AuthenticationFilter]
    [Scope("monitor-view")]
    public class MonitorService : Hub, IMonitorService
    {
        private readonly IHubContext<MonitorService> _context;
        private readonly ISessionFactory _sessionFactory;

        public MonitorService(IHubContext<MonitorService> context, ISessionFactory sessionFactory) 
        {
            _context = context;
            _sessionFactory = sessionFactory;
        }

        private async Task<IClientProxy> GetUserAsync()
        {
            var sessionId = (await _sessionFactory.GetSessionAsync())?.Id;
            return _context.Clients?.User(sessionId);
        }
        public async Task SendMatrixInitAsync(string name, Mat m, bool isBinary)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                _ = user.SendCoreAsync("OnMatrixInit", new[] { new MatrixInitDto(name, m, isBinary) });
            }
        }

        public async Task SendMatrixUpdateAsync(string name, int row, int col, double value)
        {
            var user = await GetUserAsync();
            if (user != null)
            {
                _ = user.SendCoreAsync("OnMatrixUpdate", new[] { new MatrixUpdateDto(name, row, col, value) });
            }
        }
    }
}