using FloorSweep.PathFinding;
using System.Collections.Generic;
using System.Drawing;

namespace FloorSweep.Pathfinding.TestApp
{
    public class LoadedMap
    {
        public LoadedMap(MapData data,string path,Image image)
        {
            Data = data;
            File = path;
            Image = image;
        }
        public MapData Data { get;}
        public List<long> Mean { get; } = new List<long>();
        public string File { get; }

        public Image Image { get; }
    }
}
