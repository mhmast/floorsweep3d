using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    public interface IEventHandler<T>
    {
        Task OnStatusUpdatedAsync(T status);
        Task ResetStatusAsync();
    }
}
