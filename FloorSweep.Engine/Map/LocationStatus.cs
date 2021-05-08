﻿using System;

namespace FloorSweep.Engine.Map
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

        public static string KEY = "LocationStatus";
        public LocationDeterminationStatus LocationDeterminationStatus { get; set; }

        public double AvgSpeedPixelsPerSecond { get; set; }

        public DateTime LastUpdateReceived { get; set; }

        public object Data { get; set; }

        public string Key => KEY;
    }
}