using FloorSweep.Engine.EventHandlers;
using FloorSweep.Engine.Session;
using System;

namespace FloorSweep.Engine.Map
{
    internal class LocationStatusEventHandlerFactory : IEventHandlerFactory<ILocationStatus>
    {
        private readonly Func<ISessionRepository> _repositoryFunc;

        public LocationStatusEventHandlerFactory(Func<ISessionRepository> repositoryFunc) => _repositoryFunc = repositoryFunc;
        public IEventHandler<ILocationStatus> GetEventHandler()
        => _repositoryFunc();
    }
}
