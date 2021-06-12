using System.Threading.Tasks;

namespace FloorSweep.Engine.Core
{
    public interface IDataProvider<T>
    {
        Task<T> GetDataAsync();
    }
}
