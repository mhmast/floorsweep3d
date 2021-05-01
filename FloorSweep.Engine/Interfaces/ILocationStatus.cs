using System;

namespace FloorSweep.Engine.Interfaces
{
    public interface ILocationStatus
    {
        LocationDeterminationStatus LocationDeterminationStatus { get; }
        double AvgSpeedPixelsPerSecond { get; }
        object Data { get; }
        DateTime LastUpdateReceived { get; }
    }
}
