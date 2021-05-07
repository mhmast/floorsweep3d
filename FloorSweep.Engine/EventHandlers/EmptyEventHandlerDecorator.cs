using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EmptyEventHandlerDecorator<TArg> : IEventHandlerDecorator<TArg>
    {

        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(true);
    }
}
