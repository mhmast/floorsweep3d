using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EmptyEventHandlerDecorator<TArg> : IEventHandlerDecorator<TArg>, IEventHandler<TArg>
    {

        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(true);

        Task IEventHandler<TArg>.OnStatusUpdatedAsync(TArg status) => Task.CompletedTask;

    }
}
