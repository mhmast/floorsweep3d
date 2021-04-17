using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FloorSweep.Api.Controllers.Models
{
    public class PathPoint
    {
       
        public PathPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
    public class PathDto : List<PathPoint>
    {
        public PathDto(IPath path)
        {
            path.Path.ToList().ForEach(p => Add(new PathPoint(p)));
        }
    }
}
