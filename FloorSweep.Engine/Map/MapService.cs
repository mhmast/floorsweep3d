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


        private Task<ILocationStatus> DetermineNextLocationStatusAsync(IRobotStatus status,ILocationStatus locationStatus)
        {
            return locationStatus.LocationDeterminationStatus switch
            {
                LocationDeterminationStatus.Unknown => InitSpeedTestAsync(locationStatus),
                LocationDeterminationStatus.SpeedTesting => DoSpeedTestAsync(status,locationStatus),
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
            return new LocationStatus(locationStatus) { LocationDeterminationStatus = LocationDeterminationStatus.SpeedTesting ,LastUpdateReceived = _dateTimeProvider.UtcNow};
        }

        private async Task<MapData> EnsureMapData(ISession session)
        {
            var pathFindingSession = session.PathFindingSession;
            if (pathFindingSession == null)
            {
                pathFindingSession = await StartNewPathfindingSessionAsync();
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
            var session = await _sessionRepository.GetSessionAsync();
            var mapData = await EnsureMapData(session);
            var locationStatus = await EnsureLocationStatusAsync(session);
            var nextStatus = await DetermineNextLocationStatusAsync(status,locationStatus);
            await _eventService.SendLocationStatusUpdatedAsync(nextStatus);
            return nextStatus.LocationDeterminationStatus != LocationDeterminationStatus.LocationInSync;
        }

        private async Task<LocationStatus> EnsureLocationStatusAsync(ISession session)
        {
            if (session.LocationStatus == null)
            {
                var locationStatus = new LocationStatus();
                await _eventService.SendLocationStatusUpdatedAsync(locationStatus);
                return locationStatus;
            }
            else
            {
                return new LocationStatus(session.LocationStatus);
            }
        }

        public async Task ResetStatusAsync() => await  _eventService.SendLocationStatusUpdatedAsync(new LocationStatus());
    }
}
