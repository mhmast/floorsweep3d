using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class CompositeEventHandlerDecorator<TArg> : IEventHandler<TArg>, IEventHandlerDecorator<TArg>
    {
        private readonly IEventHandlerDecorator<TArg> _thisHandler;
        private readonly IEventHandlerDecorator<TArg> _next;

        public CompositeEventHandlerDecorator(IEventHandlerDecorator<TArg> thisHandler, IEventHandlerDecorator<TArg> next = null)
        {
            _thisHandler = thisHandler;
            _next = next;
        }


        async Task<bool> IEventHandlerDecorator<TArg>.OnStatusUpdatedAsync(TArg status)
        {
            var result = await _thisHandler.OnStatusUpdatedAsync(status);
            if (_next != null && !result)
            {
                return await _next.OnStatusUpdatedAsync(status);
            }
            return result;
        }

        Task IEventHandler<TArg>.OnStatusUpdatedAsync(TArg status)
        => ((IEventHandlerDecorator<TArg>)this).OnStatusUpdatedAsync(status);
    }
}
