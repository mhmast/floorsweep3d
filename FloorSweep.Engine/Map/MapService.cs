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


        private Task<ILocationStatus> DetermineNextLocationStatusAsync(IRobotStatus status, ILocationStatus locationStatus)
        {
            return locationStatus.LocationDeterminationStatus switch
            {
                LocationDeterminationStatus.Unknown => InitSpeedTestAsync(locationStatus),
                LocationDeterminationStatus.SpeedTesting => DoSpeedTestAsync(status, locationStatus),
                _ => Task.FromResult(locationStatus)
            };
        }

        private async Task<ILocationStatus> DoSpeedTestAsync(IRobotStatus status, ILocationStatus locationStatus)
        {
            var newStatus = new LocationStatus(locationStatus);
            if(newStatus.Data == null)
            {
                newStatus.Data = status;
            }
            if (status.CurrentAction.Type == RobotActionType.Stopped)
            {
                var lastMessage = newStatus.Data as IRobotStatus;
                var traveledDistanceInMm = lastMessage.DistanceToObject - status.DistanceToObject;
                var elapsedTimeSeconds = (status.StatusDate - lastMessage.StatusDate).TotalSeconds;
                newStatus.AvgSpeedMmPerSecond = traveledDistanceInMm / elapsedTimeSeconds;
                newStatus.AvgSpeedPixelsPerSecond = newStatus.AvgSpeedMmPerSecond * PixelsPerMM;
                newStatus.Data = null;
                newStatus.LocationDeterminationStatus = LocationDeterminationStatus.Orienting;

                await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateTurnCommand(180));
                return newStatus;
            }
            newStatus.Data = status;
            return locationStatus;
        }

        private async Task<ILocationStatus> InitSpeedTestAsync(ILocationStatus locationStatus)
        {
            await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateDriveCommand());
            return new LocationStatus(locationStatus) { LocationDeterminationStatus = LocationDeterminationStatus.SpeedTesting, LastUpdateReceived = _dateTimeProvider.UtcNow };
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
            var nextStatus = await DetermineNextLocationStatusAsync(status, locationStatus);
            await _sessionRepository.SaveObjectAsync(nextStatus);
            await _eventService.SendLocationStatusUpdatedAsync(nextStatus);
            return nextStatus.LocationDeterminationStatus != LocationDeterminationStatus.LocationInSync;
        }

        private async Task<LocationStatus> EnsureLocationStatusAsync()
        {
            var sessionStatus = await _sessionRepository.GetObjectAsync<ILocationStatus>();
            if (sessionStatus == null)
            {
                var locationStatus = new LocationStatus();
                await _sessionRepository.SaveObjectAsync(locationStatus);
                return locationStatus;
            }
            else
            {
                return new LocationStatus(sessionStatus);
            }
        }

        public async Task ResetStatusAsync() => await _eventService.SendLocationStatusUpdatedAsync(new LocationStatus());
    }
}
