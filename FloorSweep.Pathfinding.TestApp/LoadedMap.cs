using FloorSweep.PathFinding;
using System.Collections.Generic;

namespace FloorSweep.Pathfinding.TestApp
{
    public class LoadedMap
    {
        public LoadedMap(MapData data,string path)
        {
            Data = data;
            File = path;
        }
        public MapData Data { get;}
        public List<long> Mean { get; } = new List<long>();
        public string File { get; }
    }
}
