using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{
    public interface IEventHandlerDecorator<T> 
    {
        Task<bool> OnStatusUpdatedAsync(T status);
        Task ResetStatusAsync();
    }
}
