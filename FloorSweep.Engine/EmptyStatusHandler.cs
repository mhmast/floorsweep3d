using FloorSweep.Engine.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine
{
    internal class EmptyStatusHandler<TArg> : IStatusUpdateHandler<TArg>
    {

        public Task<bool> OnStatusUpdatedAsync(TArg status)
        => Task.FromResult(true);
    }
}
