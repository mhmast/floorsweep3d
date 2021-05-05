using System.Threading.Tasks;

namespace FloorSweep.Engine.StatusHandlers
{
    internal class EmptyStatusHandler<TArg> : IStatusUpdateHandler<TArg>
    {

        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(true);
    }
}
