using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    internal class EmptyEventHandler<TArg> : IEventHandler<TArg>
    {

        public Task OnStatusUpdatedAsync(TArg status)
        => Task.CompletedTask;
    }
}
