using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IStatusUpdateHandler<T>
    {
        Task<bool> OnStatusUpdatedAsync(T status);
    }
}
