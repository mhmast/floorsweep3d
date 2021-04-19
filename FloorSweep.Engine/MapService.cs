using FloorSweep.Api.Interfaces;
using FloorSweep.Engine.Interfaces;
using FloorSweep.PathFinding;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Threading.Tasks;

namespace FloorSweep.Engine
{
    internal class MapService : IStatusUpdateHandler<IRobotStatus>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventService _eventService;
        private readonly IPathFindingAlgorithm _pathFindingAlgorithm;

        public MapService(
            ISessionRepository sessionRepository, 
            IEventService eventService,
            IPathFindingAlgorithm pathFindingAlgorithm)
        {
            _sessionRepository = sessionRepository;
            _eventService = eventService;
            _pathFindingAlgorithm = pathFindingAlgorithm;
        }


        private async Task<ILocationStatus> DetermineNextLocationStatusAsync(ILocationStatus status)
        {

            return status;
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
            var nextStatus = await DetermineNextLocationStatusAsync(locationStatus);
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
    }
}
