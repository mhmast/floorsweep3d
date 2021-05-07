using System;
using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EventHandlerDecoratorAdapter<T> : IEventHandlerDecorator<T>
    {
        private readonly Func<T, Task<bool>> _wrapped;

        public EventHandlerDecoratorAdapter(Func<T, Task<bool>> wrapped) => _wrapped = wrapped;

        public Task<bool> OnStatusUpdatedAsync(T status)
        => _wrapped(status);
    }
    internal class EventHandlerAdapter<T> : IEventHandler<T>
    {
        private readonly Func<T, Task> _wrapped;

        public EventHandlerAdapter(Func<T, Task> wrapped) => _wrapped = wrapped;

        public Task OnStatusUpdatedAsync(T status)
        => _wrapped(status);
    }

    public static class EventHandlerAdapterExtensions
    {
        public static IEventHandlerDecorator<T> AsEventDecorator<T>(this Func<T, Task<bool>> wrapped)
        => new EventHandlerDecoratorAdapter<T>(wrapped);
        
        public static IEventHandler<T> AsEventHandler<T>(this Func<T, Task> wrapped)
        => new EventHandlerAdapter<T>(wrapped);
    }
}
