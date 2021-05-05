using System;
using System.Linq;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EventHandlerFactory<T> : IEventHandlerFactory<T>
    {
        private readonly Func<IEventHandler<T>>[] _handlerFuncs;

        public EventHandlerFactory(params Func<IEventHandler<T>>[] handlerFuncs) => _handlerFuncs = handlerFuncs;
        IEventHandler<T> IEventHandlerFactory<T>.GetEventHandler()
        => _handlerFuncs.Any() ? _handlerFuncs.Select(f => f()).Aggregate((a, f) => new CompositeEventHandler<T>(a, f))
            : new EmptyEventHandler<T>();
    }
}
