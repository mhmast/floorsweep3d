using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EmptyEventHandler<TArg> : IEventHandler<TArg>
    {

        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(true);
    }
}
