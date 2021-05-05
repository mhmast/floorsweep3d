using FloorSweep.Api.Controllers.Models;
using FloorSweep.Engine.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FloorSweep.Api.Controllers

{

    [ApiController]
    [Authorize]
    [AuthenticationFilter]
    public class RobotController : Controller
    {
        private readonly IFloorSweepService _floorSweepService;

        public RobotController(IFloorSweepService floorSweepService)
        {
            _floorSweepService = floorSweepService;
        }



        [Authorize]
        [HttpPut("/robot/status")]
        [Scope("status_update")]
        public async Task<IActionResult> UpdateRobotStatus([FromBody] RobotStatusDto status)
        {
            await _floorSweepService.OnRobotStatusUpdatedAsync(status) ;
            return Ok();
        }
        
        [Authorize]
        [HttpPost("/robot/status")]
        [Scope("status_update")]
        public async Task<IActionResult> StartNewRobotStatus([FromBody] RobotStatusDto status)
        {
            await _floorSweepService.OnRobotStatusResetAsync(status);
            return Ok();
        }
    }
}
