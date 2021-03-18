using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Interfaces
{
    public interface IPathFindingSession
    {
        IPath FindPath(Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> debugCallback = null);
      
        Task<IPath> FindPathAsync(Action<IReadOnlyDictionary<string,Mat> , IReadOnlyDictionary<string, bool>> debugCallback = null);
    }
}
