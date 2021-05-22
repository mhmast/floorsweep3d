using FloorSweep.Engine.Map;
using LocationDeterminationStatus2 = FloorSweep.Engine.Map.LocationDeterminationStatus;
using System;

namespace FloorSweep.Api.Hubs.Dtos
{
    internal class LocationStatusUpdateDto
    {
        public LocationStatusUpdateDto(ILocationStatus status)
        {
            AvgSpeedMmPerSecond = status.AvgSpeedMmPerSecond;
            AvgSpeedPixelsPerSecond = status.AvgSpeedPixelsPerSecond;
            LocationDeterminationStatus = ConvertToDtoStatus(status.LocationDeterminationStatus);
        }

        private static LocationDeterminationStatus ConvertToDtoStatus(LocationDeterminationStatus2 locationDeterminationStatus)
        => locationDeterminationStatus switch
        {
            LocationDeterminationStatus2.LocationInSync => LocationDeterminationStatus.LocationInSync,
            LocationDeterminationStatus2.Unknown => LocationDeterminationStatus.Unknown,
            LocationDeterminationStatus2.SpeedTesting => LocationDeterminationStatus.SpeedTesting,
            LocationDeterminationStatus2.Orienting => LocationDeterminationStatus.Orienting,
            _ => throw new NotImplementedException(),
        };

        public double AvgSpeedMmPerSecond { get; }
        public double AvgSpeedPixelsPerSecond { get; }
        public LocationDeterminationStatus LocationDeterminationStatus { get; }
    }
}
