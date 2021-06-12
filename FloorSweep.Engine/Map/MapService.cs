using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;
using FloorSweep.Engine.Session;
using FloorSweep.Engine.Events;
using FloorSweep.Engine.Commands;
using FloorSweep.Math;
using FloorSweep.Engine.Core;
using System.Linq;
using System;
using FloorSweep.Engine.Config;

namespace FloorSweep.Engine.Map
{
    internal class MapService : IMapService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventService _eventService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;
        private readonly IRobotCommandFactory _robotCommandFactory;
        private readonly IDataProvider<IRobotMeta> _robotMetaProvider;
        private readonly IMapConfiguration _mapConfig;

        public MapService(
            ISessionRepository sessionRepository,
            IEventService eventService,
            IPathFindingAlgorithm pathFindingAlgorithm,
            IRobotCommandFactory robotCommandFactory,
            IDataProvider<IRobotMeta> robotMetaProvider,
            IMapConfiguration mapConfig)
        {
            _sessionRepository = sessionRepository;
            _eventService = eventService;
            _pathFindingAlgorithm = pathFindingAlgorithm;
            _robotCommandFactory = robotCommandFactory;
            _robotMetaProvider = robotMetaProvider;
            _mapConfig = mapConfig;
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

        public async Task OnStatusUpdatedAsync(IRobotStatus status)
        {
            var mapData = await EnsureMapData();
            var locationStatus = await EnsureLocationStatusAsync();
            await UpdatePositionDataAsync(status, mapData, locationStatus);
            await UpdateMapAsync(status, mapData, locationStatus);
        }

        private Task UpdateMapAsync(IRobotStatus status, MapData mapData, LocationStatus locationStatus)
        {
            var rayPixels = status.DistanceToObject * _mapConfig.PixelsPerMM;
            var loc = locationStatus.Location;
            for(var i =1;i<rayPixels;i++)
            {
                loc += locationStatus.Direction;
                mapData.Map[loc] = 
            }

        }

        private async Task UpdatePositionDataAsync(IRobotStatus status, MapData map, LocationStatus locationStatus)
        {

            if (locationStatus.LastReceivedStatus == null)
            {
                await SetInitialLocationAsync(status, map, locationStatus);
            }
            else
            {
                await (status.CurrentAction.Type switch
                {
                    RobotActionType.Stopped => new Func<Task>(() =>
                    {
                        locationStatus.LastReceivedStatus = status;
                        return Task.CompletedTask;
                    })()
                    ,
                    RobotActionType.Turned => new Func<Task>(() =>
                    {
                        locationStatus.LastReceivedStatus = status;
                        locationStatus.RotationDegrees += (int)status.CurrentAction.Data;
                        return Task.CompletedTask;
                    })()
                    ,
                    RobotActionType.Driving => new Func<Task>(async () =>
                    {
                        var elapsedSeconds = (int)(status.StatusDate - locationStatus.LastReceivedStatus.StatusDate).TotalSeconds;
                        var meta = await _robotMetaProvider.GetDataAsync();
                        var traveledPixels = meta.AvgSpeedPixelsPerSecond * elapsedSeconds;
                        locationStatus.Location += locationStatus.Direction * traveledPixels;
                        locationStatus.LastReceivedStatus = status;
                    })()
                    ,
                    _ => Task.CompletedTask
                }); ;
            }
            await _sessionRepository.SaveObjectAsync(locationStatus);

        }

        private async Task SetInitialLocationAsync(IRobotStatus status, MapData map, LocationStatus locationStatus)
        {
            locationStatus.Location = map.Map.Size / 2;
            locationStatus.RotationDegrees = 0;
            locationStatus.LastReceivedStatus = status;
            await _sessionRepository.SaveObjectAsync(locationStatus);
        }

        private async Task<LocationStatus> EnsureLocationStatusAsync()
        => await _sessionRepository.GetObjectAsync<LocationStatus>() ?? new LocationStatus();
        public async Task ResetStatusAsync()
        {
            var status = new LocationStatus();
            await _sessionRepository.SaveObjectAsync(status);
        }
    }
}
