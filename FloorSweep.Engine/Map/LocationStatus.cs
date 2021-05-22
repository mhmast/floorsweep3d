using FloorSweep.Engine.Models;
using System;

namespace FloorSweep.Engine.Map
{
    internal class LocationStatus : ILocationStatus
    {
        public LocationStatus()
        {
            LocationDeterminationStatus = LocationDeterminationStatus.Unknown;
            AvgSpeedMmPerSecond = double.NegativeInfinity;
        }
        

        public LocationDeterminationStatus LocationDeterminationStatus { get; set; }

        public double AvgSpeedMmPerSecond { get; set; }
        public IRobotStatus LastReceivedStatus { get; set; }

        public double AvgSpeedPixelsPerSecond { get; set; }
    }
}