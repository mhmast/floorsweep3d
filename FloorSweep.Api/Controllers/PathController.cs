using FloorSweep.Api;
using FloorSweep.Api.Controllers.Models;
using FloorSweep.Engine.Interfaces;
using FloorSweep.Math;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Api.Controllers

{
    [Route("/path")]
    [ApiController]
    [Authorize]
    [AuthenticationFilter]
    public class PathController : Controller
    {
       
        private readonly ILogger<PathController> _logger;
        private readonly ISessionFactory _sessionFactory;
        private readonly IFloorSweepService _floorSweepService;

        public PathController(ILogger<PathController> logger
            ,ISessionFactory sessionFactory
            ,IFloorSweepService floorSweepService)
        {
            _logger = logger;
            _sessionFactory = sessionFactory;
            _floorSweepService = floorSweepService;
        }



        [Authorize]
        [HttpPost()]
        [Scope("path_search")]
        public async Task<IActionResult> RegisterMonitor([FromBody] PathParametersDto pathParams)
        {
            var ses = await _sessionFactory.GetSessionAsync();
            var path = await _floorSweepService.FindPathAsync(ses, pathParams);
            return Ok(new PathDto(path));
        }
    }
}
