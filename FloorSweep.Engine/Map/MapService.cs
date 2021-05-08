using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FloorSweep.Engine.Session;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Commands;
using FloorSweep.Engine.Models;

namespace FloorSweep.Engine.Map
{
    internal class MapService : IMapService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventService _eventService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;
        private readonly IRobotCommandFactory _robotCommandFactory;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const string PathfindingSessionKey = "PathfindingSession";
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

        const int MaxSamplesForSpeedTest = 4;
        private async Task<ILocationStatus> DoSpeedTestAsync(IRobotStatus status, ILocationStatus locationStatus)
        {
            var newStatus = new LocationStatus(locationStatus);
            if (status.CurrentAction.Type == RobotActionType.Turned)
            {
                await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateDriveCommand());
                return newStatus;
            }
            if (status.CurrentAction.Type == RobotActionType.Stopped)
            {
                if (newStatus.Data is not List<int> speedAvgs)
                {
                    speedAvgs = new List<int>();
                    newStatus.Data = speedAvgs;
                }
                if (speedAvgs.Count == MaxSamplesForSpeedTest)
                {
                    newStatus.AvgSpeedPixelsPerSecond = speedAvgs.Average();
                    newStatus.Data = null;
                    newStatus.LocationDeterminationStatus = LocationDeterminationStatus.Orienting;
                    return newStatus;
                }
                var utcNow = _dateTimeProvider.UtcNow;
                var time = (utcNow - newStatus.LastUpdateReceived).Seconds;
                speedAvgs.Add(time);
                newStatus.LastUpdateReceived = utcNow;
                await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateTurnCommand(180));
                return newStatus;
            }
            return locationStatus;
        }

        private async Task<ILocationStatus> InitSpeedTestAsync(ILocationStatus locationStatus)
        {
            await _eventService.SendRobotCommandAsync(_robotCommandFactory.CreateDriveCommand());
            return new LocationStatus(locationStatus) { LocationDeterminationStatus = LocationDeterminationStatus.SpeedTesting, LastUpdateReceived = _dateTimeProvider.UtcNow };
        }

        private async Task<MapData> EnsureMapData()
        {
            var pathFindingSession = await _sessionRepository.GetObjectAsync<IPathFindingSession>(PathfindingSessionKey);
            if (pathFindingSession == null)
            {
                pathFindingSession = await StartNewPathfindingSessionAsync();
                await _sessionRepository.SaveObjectAsync(PathfindingSessionKey, pathFindingSession);
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
            var sessionStatus = await _sessionRepository.GetObjectAsync<ILocationStatus>(LocationStatus.KEY);
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
