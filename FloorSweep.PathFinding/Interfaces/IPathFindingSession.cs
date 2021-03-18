using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Interfaces
{
    public interface IPathFindingSession
    {
        IPath FindPath();
        Task<IPath> FindPathAsync();
    }
}
