using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;
using FloorSweep.Engine.Session;
using FloorSweep.Math;
using FloorSweep.Engine.Core;
using System;
using FloorSweep.Engine.Config;

namespace FloorSweep.Engine.Map
{
    internal class MapService : IMapService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;
        private readonly IDataProvider<IRobotMeta> _robotMetaProvider;
        private readonly IMapConfiguration _mapConfig;

        public MapService(
            ISessionRepository sessionRepository,
            IPathFindingAlgorithm pathFindingAlgorithm,
            IDataProvider<IRobotMeta> robotMetaProvider,
            IMapConfiguration mapConfig)
        {
            _sessionRepository = sessionRepository;
            _pathFindingAlgorithm = pathFindingAlgorithm;
            _robotMetaProvider = robotMetaProvider;
            _mapConfig = mapConfig;
        }

        private async Task<IPathFindingSession> EnsurePathFindingSession()
        {
            var pathFindingSession = await _sessionRepository.GetObjectAsync<IPathFindingSession>();
            if (pathFindingSession == null)
            {
                pathFindingSession = await StartNewPathfindingSessionAsync();
            }
            return pathFindingSession;
        }

        private Task<IPathFindingSession> StartNewPathfindingSessionAsync()
        {
            var map = MapData.Empty();
            return Task.FromResult(_pathFindingAlgorithm.CreateSession(map));
        }

        public async Task OnStatusUpdatedAsync(IRobotStatus status)
        {
            var pfSession = await EnsurePathFindingSession();
            var locationStatus = await EnsureLocationStatusAsync();
            await UpsertLocationAsync(status, pfSession, locationStatus);
            await UpdateMapAsync(status, pfSession, locationStatus);
        }

        private async Task UpdateMapAsync(IRobotStatus status, IPathFindingSession pfSession, LocationStatus locationStatus)
        {
            var rayPixels = status.DistanceToObject * _mapConfig.PixelsPerMM;
            var loc = locationStatus.Location;
            var map = pfSession.MapData.Map;
            for (var i = 1; i <= rayPixels; i++)
            {
                loc += locationStatus.Direction;
                map[loc] = i == rayPixels ? Constants.Obstruction : Constants.Open;
            }
            await _sessionRepository.SaveObjectAsync(pfSession);
        }

        private async Task UpsertLocationAsync(IRobotStatus status, IPathFindingSession pfSession, LocationStatus locationStatus)
        {

            if (locationStatus.LastReceivedStatus == null)
            {
                await InitLocationAsync(status, pfSession.MapData.Map.Size, locationStatus);
            }
            else
            {
                await UpdateLocationAsync(status, locationStatus);
            }
            await _sessionRepository.SaveObjectAsync(locationStatus);

        }

        private async Task UpdateLocationAsync(IRobotStatus status, LocationStatus locationStatus)
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

        private async Task InitLocationAsync(IRobotStatus status, Point mapSize, LocationStatus locationStatus)
        {
            locationStatus.Location = mapSize / 2;
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
