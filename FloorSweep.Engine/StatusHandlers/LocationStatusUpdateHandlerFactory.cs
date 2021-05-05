using FloorSweep.Engine.Map;
using FloorSweep.Engine.Session;
using System;

namespace FloorSweep.Engine.StatusHandlers
{
    internal class LocationStatusUpdateHandlerFactory : IStatusUpdateHandlerFactory<ILocationStatus>
    {
        private readonly Func<ISessionRepository> _repositoryFunc;

        public LocationStatusUpdateHandlerFactory(Func<ISessionRepository> repositoryFunc) => _repositoryFunc = repositoryFunc;
        public IStatusUpdateHandler<ILocationStatus> GetStatusUpdateHandler()
        => _repositoryFunc();
    }
}
