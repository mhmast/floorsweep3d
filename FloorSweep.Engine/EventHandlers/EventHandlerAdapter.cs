using System;
using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EventHandlerAdapter<T> : IEventHandler<T>
    {
        private readonly Func<T, Task<bool>> _wrapped;

        public EventHandlerAdapter(Func<T, Task<bool>> wrapped) => _wrapped = wrapped;

        public Task<bool> OnStatusUpdatedAsync(T status)
        => _wrapped(status);
    }
}
