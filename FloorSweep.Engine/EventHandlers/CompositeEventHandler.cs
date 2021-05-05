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


        public async Task<bool> OnStatusUpdatedAsync(TArg status)
        {
            var result = await _thisHandler.OnStatusUpdatedAsync(status);
            if (_next != null && !result)
            {
                return await _next.OnStatusUpdatedAsync(status);
            }
            return result;
        }
    }
}
