//using FloorSweep.Math;
//using System;

//namespace FloorSweep.PathFinding
//{
//    public class LoadMap
//    {
//        public static MapData DoLoadMap(MapData mapData, int scaling = 1)
//        {
//            //var mapData = Segmentation.DoSegmentation(data, 0.1, 0.9, 0.5);
//            var robot_xy = mapData.Start;
//            var target_xy = mapData.Target;
//            var map = mapData.Map;
//            if (scaling != 1)
//            {
//                map = SimplifyMap.DoSimplifyMap(map, scaling);
//            }
//            var @out = new MapData();
//            @out.Map = Mat.Zeros(5, map.Cols);
//            @out.Map.AddBottom(map);
//            @out.Map.AddBottom(Mat.Zeros(6, map.Cols));
//            var m = @out.Map.T().ToMat();
//            @out.Map = Mat.Zeros(5, m.Cols);
//            @out.Map.AddBottom(m);
//            @out.Map.AddBottom(Mat.Zeros(6, m.Cols));
//            @out.Map = @out.Map.T();

//            @out.Start = robot_xy / scaling + 5;
//            @out.Target = target_xy / scaling + 5;
//            @out.OriginalImage = mapData.OriginalImage;
//            return @out;
//        }
//    }
//}
