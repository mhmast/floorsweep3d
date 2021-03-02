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
            @out.Map = Mat.Zeros(5, map.Cols,map.Type());
            @out.Map.AddBottom(map);
            @out.Map.AddBottom(Mat.Zeros(6, map.Cols, map.Type()));
            var m = @out.Map.T().ToMat();
            @out.Map = Mat.Zeros(5, m.Cols, map.Type());
            @out.Map.AddBottom(m);
            @out.Map.AddBottom(Mat.Zeros(6, m.Cols, map.Type()));
            @out.Map = @out.Map.T();

            @out.Start = (robot_xy / scaling).ToMat().Floor().Plus( new Mat(1, 2, robot_xy.Type(), 5)) ;
            @out.Target = (target_xy / scaling).ToMat().Floor().Plus( new Mat(1, 2, target_xy.Type(), 5)) ;
            @out.Start = @out.Start.T();
            @out.Target = @out.Target.T();
            @out.Image = mapData.Image;
            return @out;
        }
    }
}
