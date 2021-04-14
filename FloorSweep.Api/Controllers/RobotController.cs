using FloorSweep.Api;
using FloorSweep.Engine.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Api.Controllers

{
    [Route("/robot")]
    [ApiController]
    [Authorize]
    [AuthenticationFilter]
    public class RobotController : Controller
    {
       
        private readonly ILogger _logger;
        private readonly ISessionFactory _sessionFactory;
        private readonly IFloorSweepService _floorSweepService;

        public RobotController(ILogger<RobotController> logger
            ,ISessionFactory sessionFactory
            ,IFloorSweepService floorSweepService)
        {
            _logger = logger;
            _sessionFactory = sessionFactory;
            _floorSweepService = floorSweepService;
        }



        [Authorize]
        [HttpPut("/vision/distance/{distance:int}")]
        [Scope("vision_update")]
        public async Task<IActionResult> UpdateRobotVision([FromRoute] int distance)
        {
            return Ok();
        }
    }
}
