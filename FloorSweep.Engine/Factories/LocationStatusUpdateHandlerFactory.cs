using FloorSweep.Api.Interfaces;
using FloorSweep.Engine.Interfaces;
using System;

namespace FloorSweep.Engine.Factories
{
    internal class LocationStatusUpdateHandlerFactory : IStatusUpdateHandlerFactory<ILocationStatus>
    {
        private readonly Func<ISessionRepository> _repositoryFunc;

        public LocationStatusUpdateHandlerFactory(Func<ISessionRepository> repositoryFunc) => _repositoryFunc = repositoryFunc;
        public IStatusUpdateHandler<ILocationStatus> GetStatusUpdateHandler()
        => _repositoryFunc();
    }
}
