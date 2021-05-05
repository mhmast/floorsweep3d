using FloorSweep.Api.Controllers.Models;
using FloorSweep.Engine.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FloorSweep.Api.Controllers

{
    [Route("/path")]
    [ApiController]
    [Authorize]
    [AuthenticationFilter]
    public class PathController : Controller
    {
       
        private readonly IFloorSweepService _floorSweepService;

        public PathController(IFloorSweepService floorSweepService)
        {
            _floorSweepService = floorSweepService;
        }

        [Authorize]
        [HttpPost()]
        [Scope("path_search")]
        public async Task<IActionResult> RegisterMonitor([FromBody] PathParametersDto pathParams)
        {
            var path = await _floorSweepService.FindPathAsync( pathParams);
            return Ok(new PathDto(path));
        }
    }
}
