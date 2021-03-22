using FloorSweep.Api;
using FloorSweep.Engine.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Api.Controllers
{
    [Authorize]
    [AuthenticationFilter]
    [ApiController]
    [Route("monitors")]
    public class MonitorController : ControllerBase
    {
       
        private readonly ILogger<MonitorController> _logger;
        private readonly ISessionFactory _sessionFactory;
        private readonly IMonitorService _monitorService;

        public MonitorController(ILogger<MonitorController> logger
            ,ISessionFactory sessionFactory
            ,IMonitorService monitorService)
        {
            _logger = logger;
            _sessionFactory = sessionFactory;
            _monitorService = monitorService;
        }

        [Authorize]
        [HttpPost()]
        [Scope("monitor-view")]
        public async Task<IActionResult> RegisterMonitor(string ip)
        {
            var ses = await _sessionFactory.GetSessionAsync();
            await _monitorService.RegisterMonitorAsync(ses, ip);
            return Ok();
        }
    }
}
