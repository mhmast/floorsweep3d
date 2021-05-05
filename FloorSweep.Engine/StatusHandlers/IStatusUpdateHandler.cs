using System.Threading.Tasks;

namespace FloorSweep.Engine.StatusHandlers
{
    public interface IStatusUpdateHandler<T>
    {
        Task<bool> OnStatusUpdatedAsync(T status);
    }
}
