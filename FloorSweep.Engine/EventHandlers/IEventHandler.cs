using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    public interface IEventHandler<T>
    {
        Task<bool> OnStatusUpdatedAsync(T status);
    }
}
