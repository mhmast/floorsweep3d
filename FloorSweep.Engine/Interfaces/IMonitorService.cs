using FloorSweep.Math;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IMonitorService
    {
        Task SendMatrixInitAsync(string name,Mat m,bool isBinary);
        Task SendMatrixUpdateAsync(string key, int row, int col, double value);
    }
}