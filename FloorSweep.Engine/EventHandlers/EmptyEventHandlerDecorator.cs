using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EmptyEventHandlerDecorator<TArg> : IEventHandlerDecorator<TArg>, IEventHandler<TArg>
    {
        private readonly bool _returnValue;

        public EmptyEventHandlerDecorator(bool returnValue)
        {
            _returnValue = returnValue;
        }
        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(_returnValue);

        public Task ResetStatusAsync()
        => Task.CompletedTask;

        Task IEventHandler<TArg>.OnStatusUpdatedAsync(TArg status) => Task.CompletedTask;

    }
}
