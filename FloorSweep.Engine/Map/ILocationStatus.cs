using FloorSweep.Engine.Session;
using System;

namespace FloorSweep.Engine.Map
{
    public interface ILocationStatus : IKeyable
    {
        LocationDeterminationStatus LocationDeterminationStatus { get; }
        double AvgSpeedPixelsPerSecond { get; }
        object Data { get; }
        DateTime LastUpdateReceived { get; }
    }
}
