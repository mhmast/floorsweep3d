using FloorSweep.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloorSweep.Api.Hubs.Dtos
{
    internal class LocationStatusUpdateDto
    {
        public LocationStatusUpdateDto(ILocationStatus status)
        {
            AvgSpeedPixelsPerSecond = status.AvgSpeedPixelsPerSecond;
            LastUpdateReceived = status.LastUpdateReceived;
            LocationDeterminationStatus = ConvertToDtoStatus(status.LocationDeterminationStatus);
        }

        private LocationDeterminationStatus ConvertToDtoStatus(Engine.Interfaces.LocationDeterminationStatus locationDeterminationStatus)
        => locationDeterminationStatus switch
        {
            Engine.Interfaces.LocationDeterminationStatus.LocationInSync => LocationDeterminationStatus.LocationInSync,
            Engine.Interfaces.LocationDeterminationStatus.Unknown => LocationDeterminationStatus.Unknown,
            Engine.Interfaces.LocationDeterminationStatus.SpeedTesting => LocationDeterminationStatus.SpeedTesting,
            Engine.Interfaces.LocationDeterminationStatus.Orienting => LocationDeterminationStatus.Orienting,
            _ => throw new NotImplementedException(),
        };

        public double AvgSpeedPixelsPerSecond { get; }
        public DateTime LastUpdateReceived { get; }
        public LocationDeterminationStatus LocationDeterminationStatus { get; }
    }
}
