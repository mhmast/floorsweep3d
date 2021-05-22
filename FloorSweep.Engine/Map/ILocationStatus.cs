using FloorSweep.Engine.Session;
using System;

namespace FloorSweep.Engine.Map
{
    
    public interface ILocationStatus
    {
        LocationDeterminationStatus LocationDeterminationStatus { get; }
        double AvgSpeedMmPerSecond { get; }
        double AvgSpeedPixelsPerSecond { get; }
        object Data { get; }
        DateTime LastUpdateReceived { get; }
    }
}
