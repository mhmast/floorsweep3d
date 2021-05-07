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


        async Task IEventHandler<TArg>.OnStatusUpdatedAsync(TArg status)
        {
            await _thisHandler.OnStatusUpdatedAsync(status);
            await _next?.OnStatusUpdatedAsync(status);
        }

    }
}
