using FloorSweep.Engine.Interfaces;
using System;

namespace FloorSweep.Engine
{
    internal class LocationStatus : ILocationStatus
    {
        public LocationStatus()
        {
            LocationDeterminationStatus = LocationDeterminationStatus.Unknown;
            AvgSpeedPixelsPerSecond = double.NegativeInfinity;
            LastUpdateReceived = DateTime.MinValue;
        }
        
        public LocationStatus(ILocationStatus status)
        {
            LocationDeterminationStatus = status.LocationDeterminationStatus;
            AvgSpeedPixelsPerSecond = status.AvgSpeedPixelsPerSecond;
            LastUpdateReceived = status.LastUpdateReceived;
            Data = status.Data;
        }

        public LocationDeterminationStatus LocationDeterminationStatus { get; set; }

        public double AvgSpeedPixelsPerSecond { get; set; }

        public DateTime LastUpdateReceived { get; set; }

        public object Data { get; set; }
    }
}