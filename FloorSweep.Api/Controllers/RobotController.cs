using FloorSweep.Api;
using FloorSweep.Api.Controllers.Models;
using FloorSweep.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Api.Controllers

{
    [Route("/robot")]
    [ApiController]
    [Authorize]
    [AuthenticationFilter]
    public class RobotController : Controller
    {
        private readonly IStatusService _statusService;

        public RobotController(IStatusService statusService)
        {
            _statusService = statusService;
        }



        [Authorize]
        [HttpPut("/status")]
        [Scope("status_update")]
        public async Task<IActionResult> UpdateRobotStatus([FromBody] RobotStatusDto status)
        {
            await _statusService.UpdateRobotStatusAsync(status);
            return Ok();
        }
    }
}
