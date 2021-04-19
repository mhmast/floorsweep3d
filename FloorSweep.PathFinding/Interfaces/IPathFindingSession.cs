using FloorSweep.Math;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding.Interfaces
{
    public interface IPathFindingSession
    {
        MapData MapData { get; }
        Task<IPath> FindPathAsync(Point start,Point end, Func<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>, Task> debugCallback = null);
    }
}
