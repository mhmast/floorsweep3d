using System;

namespace FloorSweep.Engine.Map
{
    internal class LocationStatus : ILocationStatus
    {
        public LocationStatus()
        {
            LocationDeterminationStatus = LocationDeterminationStatus.Unknown;
            AvgSpeedMmPerSecond = double.NegativeInfinity;
            LastUpdateReceived = DateTime.MinValue;
        }
        
        public LocationStatus(ILocationStatus status)
        {
            LocationDeterminationStatus = status.LocationDeterminationStatus;
            AvgSpeedMmPerSecond = status.AvgSpeedMmPerSecond;
            LastUpdateReceived = status.LastUpdateReceived;
            Data = status.Data;
        }

        public LocationDeterminationStatus LocationDeterminationStatus { get; set; }

        public double AvgSpeedMmPerSecond { get; set; }

        public DateTime LastUpdateReceived { get; set; }

        public object Data { get; set; }

        public double AvgSpeedPixelsPerSecond { get; set; }
    }
}