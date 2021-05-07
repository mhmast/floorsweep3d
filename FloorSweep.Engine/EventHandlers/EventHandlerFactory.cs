using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            => (IEventHandler<T>)(decorators.Any() ? decorators.Select(f => f()).Aggregate((a, f) => new CompositeEventHandlerDecorator<T>(a, f))
            : new EmptyEventHandlerDecorator<T>());

        private static IEventHandler<T> Wrap(IEnumerable<Func<IEventHandler<T>>> interceptors)
            => interceptors.Any() ? interceptors.Select(f => f()).Aggregate((a, f) => new CompositeEventHandler<T>(a, f))
            : new EmptyEventHandler<T>();
        IEventHandler<T> IEventHandlerFactory<T>.GetEventHandler() => new CompositeEventHandler<T>(Wrap(_interceptors), Wrap(_decorators));

        public class Builder
        {
            public static EventHandlerFactoryBuilder<T> WithInterceptors(params Func<T,Task>[] interceptors)
            => new (interceptors);
            
            public static EventHandlerFactoryBuilder<T> WithDecorators(params Func<T, Task<bool>>[] decorators)
            => new (decorators);

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
        public EventHandlerFactoryBuilder(IEnumerable<Func<T, Task<bool>>> decorators)
        {
            WithDecorators(decorators.ToArray());
        }

        public EventHandlerFactoryBuilder(IEnumerable<Func<T, Task>> interceptors)
        {
            WithInterceptors(interceptors.ToArray());
        }

        public EventHandlerFactoryBuilder<T> WithDecorators(params Func<T, Task<bool>>[] decorators)
        {
            _decorators.AddRange(decorators.Select(d => new Func<IEventHandlerDecorator<T>>(d.AsEventDecorator)));
            return this;
        }

        public EventHandlerFactoryBuilder<T> WithInterceptors(params Func<T, Task>[] interceptors)
        {
            _interceptors.AddRange(interceptors.Select(d => new Func<IEventHandler<T>>(d.AsEventHandler)));
            return this;
        }
        public IEventHandlerFactory<T> Build() => new EventHandlerFactory<T>(_interceptors, _decorators);
    
    }
}
