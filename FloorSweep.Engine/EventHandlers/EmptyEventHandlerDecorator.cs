using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EmptyEventHandlerDecorator<TArg> : IEventHandlerDecorator<TArg>, IEventHandler<TArg>
    {

        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(true);

        public Task ResetStatusAsync()
        => Task.CompletedTask;

        Task IEventHandler<TArg>.OnStatusUpdatedAsync(TArg status) => Task.CompletedTask;

    }
}
