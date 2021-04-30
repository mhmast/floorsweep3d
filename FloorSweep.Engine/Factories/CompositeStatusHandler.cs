using FloorSweep.Engine.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Factories
{
    internal class CompositeStatusHandler<TArg> : IStatusUpdateHandler<TArg>
    {
        private readonly IStatusUpdateHandler<TArg> _thisHandler;
        private readonly IStatusUpdateHandler<TArg> _next;

        public CompositeStatusHandler(IStatusUpdateHandler<TArg> thisHandler, IStatusUpdateHandler<TArg> next = null)
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
