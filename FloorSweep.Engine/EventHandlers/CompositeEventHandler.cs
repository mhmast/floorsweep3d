using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class CompositeEventHandler<TArg> : IEventHandler<TArg> 
    {
        private readonly IEventHandler<TArg> _thisHandler;
        private readonly IEventHandler<TArg> _next;

        public CompositeEventHandler(IEventHandler<TArg> thisHandler, IEventHandler<TArg> next = null)
        {
            _thisHandler = thisHandler;
            _next = next;
        }

        public CompositeEventHandler() : this(new EmptyEventHandler<TArg>())
        {
        }

        async Task IEventHandler<TArg>.ResetStatusAsync()
        {
            await _thisHandler.ResetStatusAsync();
            await (_next?.ResetStatusAsync()??Task.CompletedTask);
        }

        async Task IEventHandler<TArg>.OnStatusUpdatedAsync(TArg status)
        {
            await _thisHandler.OnStatusUpdatedAsync(status);
            await (_next?.OnStatusUpdatedAsync(status)??Task.CompletedTask);
        }

    }
}
