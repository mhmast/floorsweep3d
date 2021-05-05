using System.Collections.Generic;
using System.Drawing;

namespace FloorSweep.PathFinding.TestApp
{
    public class LoadedMap
    {
        public LoadedMap(Math.Point start, Math.Point end, MapData data,string path,Image image)
        {
            Start = start;
            End = end;
            Data = data;
            File = path;
            Image = image;
        }

        public Math.Point Start { get; }
        public Math.Point End { get; }
        public MapData Data { get;}
        public List<long> Mean { get; } = new List<long>();
        public string File { get; }

        public Image Image { get; }
    }
}
