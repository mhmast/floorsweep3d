using FloorSweep.Api;
using FloorSweep.Api.Controllers.Models;
using FloorSweep.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Api.Controllers

{
    
    [ApiController]
    [Authorize]
    [AuthenticationFilter]
    public class RobotController : Controller
    {
        private readonly IEventService _eventService;

        public RobotController(IEventService eventService)
        {
            _eventService = eventService;
        }



        [Authorize]
        [HttpPut("/robot/status")]
        [Scope("status_update")]
        public async Task<IActionResult> UpdateRobotStatus([FromBody] RobotStatusDto status)
        {
            await _eventService.SendRobotStatusUpdateAsync(status);
            return Ok();
        }
    }
}
