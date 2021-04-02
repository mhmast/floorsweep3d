using FloorSweep.PathFinding.Interfaces;
using System.Threading.Tasks;

namespace FloorSweep.Engine.Interfaces
{
    public interface IFloorSweepService
    {
        Task<IPath> FindPathAsync(ISession session, IPathFindingParameters path);
    }
}
