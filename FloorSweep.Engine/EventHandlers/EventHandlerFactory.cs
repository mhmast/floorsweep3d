using System;
using System.Collections.Generic;
using System.Linq;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EventHandlerFactory<T> : IEventHandlerFactory<T>
    {
        private readonly IEnumerable<Func<IEventHandler<T>>> _interceptors;
        private readonly IEnumerable<Func<IEventHandlerDecorator<T>>> _decorators;

        public EventHandlerFactory(IEnumerable<Func<IEventHandler<T>>> interceptors, IEnumerable<Func<IEventHandlerDecorator<T>>> decorators)
        {
            _interceptors = interceptors;
            _decorators = decorators;
        }

        private static IEventHandler<T> Wrap(IEnumerable<Func<IEventHandlerDecorator<T>>> decorators)
            => (IEventHandler<T>)(decorators.Any() ? decorators.Select(f => f()).Aggregate(new CompositeEventHandlerDecorator<T>(false), (a, f) => new CompositeEventHandlerDecorator<T>(a, f))
            : new EmptyEventHandlerDecorator<T>(true));

        private static IEventHandler<T> Wrap(IEnumerable<Func<IEventHandler<T>>> interceptors)
            => interceptors.Any() ? interceptors.Select(f => f()).Aggregate(new CompositeEventHandler<T>(), (a, f) => new CompositeEventHandler<T>(a, f))
            : new EmptyEventHandler<T>();
        IEventHandler<T> IEventHandlerFactory<T>.GetEventHandler() => new CompositeEventHandler<T>(Wrap(_interceptors), Wrap(_decorators));

        public class Builder
        {
            public static EventHandlerFactoryBuilder<T> WithInterceptors(params Func<IEventHandler<T>>[] interceptors)
            => new(interceptors);

            public static EventHandlerFactoryBuilder<T> WithDecorators(params Func<IEventHandlerDecorator<T>>[] decorators)
            => new(decorators);

            public static IEventHandlerFactory<T> Build() => new EventHandlerFactoryBuilder<T>().Build();
        }
    }

    internal class EventHandlerFactoryBuilder<T>
    {
        private readonly List<Func<IEventHandler<T>>> _interceptors = new();
        private readonly List<Func<IEventHandlerDecorator<T>>> _decorators = new();

        public EventHandlerFactoryBuilder()
        {

        }
        public EventHandlerFactoryBuilder(IEnumerable<Func<IEventHandlerDecorator<T>>> decorators)
        {
            WithDecorators(decorators.ToArray());
        }

        public EventHandlerFactoryBuilder(IEnumerable<Func<IEventHandler<T>>> interceptors)
        {
            WithInterceptors(interceptors.ToArray());
        }

        public EventHandlerFactoryBuilder<T> WithDecorators(params Func<IEventHandlerDecorator<T>>[] decorators)
        {
            _decorators.AddRange(decorators);
            return this;
        }

        public EventHandlerFactoryBuilder<T> WithInterceptors(params Func<IEventHandler<T>>[] interceptors)
        {
            _interceptors.AddRange(interceptors.Select(d => new Func<IEventHandler<T>>(d)));
            return this;
        }
        public IEventHandlerFactory<T> Build() => new EventHandlerFactory<T>(_interceptors, _decorators);

    }
}
