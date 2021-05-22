namespace FloorSweep.Engine.Map
{

    public interface ILocationStatus
    {
        LocationDeterminationStatus LocationDeterminationStatus { get; }
        double AvgSpeedMmPerSecond { get; }
        double AvgSpeedPixelsPerSecond { get; }
    }
}
