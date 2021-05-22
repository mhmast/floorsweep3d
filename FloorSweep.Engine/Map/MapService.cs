using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FloorSweep.Engine.Session;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Models;
using System;

namespace FloorSweep.Engine.Map
{
    internal class MapService : IMapService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventService _eventService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;
        private readonly IRobotCommandFactory _robotCommandFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const double PixelsPerMM = 0.1;
        public MapService(
            ISessionRepository sessionRepository,
            IEventService eventService,
            IPathFindingAlgorithm pathFindingAlgorithm,
            IRobotCommandFactory robotCommandFactory,
            IDateTimeProvider dateTimeProvider)
        {
            _sessionRepository = sessionRepository;
            _eventService = eventService;
            _pathFindingAlgorithm = pathFindingAlgorithm;
            _robotCommandFactory = robotCommandFactory;
            _dateTimeProvider = dateTimeProvider;
        }


        private Task DetermineNextLocationStatusAsync(IRobotStatus status, LocationStatus locationStatus)
        {
            return locationStatus.LocationDeterminationStatus switch
            {
                LocationDeterminationStatus.Unknown => InitSpeedTestAsync(locationStatus),
                LocationDeterminationStatus.SpeedTesting => DoSpeedTestAsync(status, locationStatus),
                LocationDeterminationStatus.Orienting => DoSpeedTestAsync(status, locationStatus),
                _ => Task.CompletedTask
            };
        }

        private Task DoSpeedTestAsync(IRobotStatus status, LocationStatus locationStatus)
        {
            if (status.DistanceToObject == -1)
            {
                return Task.CompletedTask;
            }
            if (locationStatus.LastReceivedStatus == null)
            {
                locationStatus.LastReceivedStatus = status;
            }
            else
            {
                var lastMessage = locationStatus.LastReceivedStatus;
                var traveledDistanceInMm = lastMessage.DistanceToObject - status.DistanceToObject;
                var elapsedTimeSeconds = (status.StatusDate - lastMessage.StatusDate).TotalSeconds;
                locationStatus.AvgSpeedMmPerSecond = traveledDistanceInMm / elapsedTimeSeconds;
                locationStatus.AvgSpeedPixelsPerSecond = locationStatus.AvgSpeedMmPerSecond * PixelsPerMM;
                locationStatus.LastReceivedStatus = null;
                locationStatus.LocationDeterminationStatus = LocationDeterminationStatus.Orienting;
            }
            return Task.CompletedTask;
        }

        private async Task InitSpeedTestAsync(LocationStatus locationStatus)
        {
            await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateDriveCommand());
            locationStatus.LocationDeterminationStatus = LocationDeterminationStatus.SpeedTesting;
        }

        private async Task<MapData> EnsureMapData()
        {
            var pathFindingSession = await _sessionRepository.GetObjectAsync<IPathFindingSession>();
            if (pathFindingSession == null)
            {
                pathFindingSession = await StartNewPathfindingSessionAsync();
                await _sessionRepository.SaveObjectAsync(pathFindingSession);
            }
            return pathFindingSession.MapData;
        }

        private Task<IPathFindingSession> StartNewPathfindingSessionAsync()
        {
            var map = MapData.Empty();
            return Task.FromResult(_pathFindingAlgorithm.CreateSession(map));
        }

        public async Task<bool> OnStatusUpdatedAsync(IRobotStatus status)
        {
            var mapData = await EnsureMapData();
            var locationStatus = await EnsureLocationStatusAsync();
            await DetermineNextLocationStatusAsync(status, locationStatus);
            await _sessionRepository.SaveObjectAsync(locationStatus);
            await _eventService.SendLocationStatusUpdatedAsync(locationStatus);
            return locationStatus.LocationDeterminationStatus != LocationDeterminationStatus.LocationInSync;
        }

        private async Task<LocationStatus> EnsureLocationStatusAsync()
        => await _sessionRepository.GetObjectAsync<LocationStatus>() ?? new LocationStatus();
        public async Task ResetStatusAsync()
        {
            var status = new LocationStatus();
            await _sessionRepository.SaveObjectAsync(status);
            await _eventService.SendLocationStatusUpdatedAsync(status);
        }
    }
}
