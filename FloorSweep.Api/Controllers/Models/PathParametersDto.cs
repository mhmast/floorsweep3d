using FloorSweep.Engine.Interfaces;
using FloorSweep.Math;

namespace FloorSweep.Api.Controllers.Models
{
    public class PathParametersDto : IPathFindingParameters
    {
        public int Start_X { get; set; }
        public int Start_Y { get; set; }
        public int End_X { get; set; }
        public int End_Y { get; set; }

        Point IPathFindingParameters.Start => new Point(Start_X, Start_Y);

        Point IPathFindingParameters.Target => new Point(End_X, End_Y);
    }
}
