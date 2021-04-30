using FloorSweep.Api.Interfaces;
using FloorSweep.Engine.Interfaces;
using System;

namespace FloorSweep.Engine.Factories
{
    internal class RobotStatusUpdateHandlerFactory : IStatusUpdateHandlerFactory<IRobotStatus>
    {
        private readonly Func<ISessionRepository> _sessionRepositoryFunc;
        private readonly Func<IMapService> _mapServiceFunc;

        public RobotStatusUpdateHandlerFactory(Func<ISessionRepository> sessionRepositoryFunc,Func<IMapService> mapServiceFunc)
        {
            _sessionRepositoryFunc = sessionRepositoryFunc;
            _mapServiceFunc = mapServiceFunc;
        }
        IStatusUpdateHandler<IRobotStatus> IStatusUpdateHandlerFactory<IRobotStatus>.GetStatusUpdateHandler()
        => new CompositeStatusHandler<IRobotStatus>(_sessionRepositoryFunc(), _mapServiceFunc());
    }
}
