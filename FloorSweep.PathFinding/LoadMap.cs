using OpenCvSharp;
using System;

namespace FloorSweep.PathFinding
{
    public class LoadMap
    {
        public static MapData DoLoadMap(string name, int scaling = 1)
        {
            var mapData = Segmentation.DoSegmentation(name, 0.1, 0.9, 0.5);
            var robot_xy = mapData.Start;
            var target_xy = mapData.Target;
            var map = mapData.Map;
            if (scaling != 1)
            {
                map = SimplifyMap.DoSimplifyMap(map, scaling);
            }
            var a = map.Rows;
            var b = map.Cols;
            var @out = new MapData();
            @out.Map = Mat.Zeros(a + 10, b + 10);
            @out.Map[5, a - 6, 5, b - 6] = map;

            @out.Start = (robot_xy / scaling).ToMat().Floor()+ new Mat(1, 2, @out.Target.Type(), 5) ;
            @out.Target = (target_xy / scaling).ToMat().Floor() + new Mat(1, 2, @out.Target.Type(), 5) ;
            return @out;
        }
    }
}
