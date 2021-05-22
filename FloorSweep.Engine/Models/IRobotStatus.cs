using FloorSweep.Engine.Session;
using System;

namespace FloorSweep.Engine.Models
{
    [SessionSaveable]
    public interface IRobotStatus
    {
        DateTime StatusDate { get; }
        IRobotAction CurrentAction { get; }
        int DistanceToObject { get; }
    }
}
