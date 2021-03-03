//using OpenCvSharp;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Text;

//namespace FloorSweep.PathFinding
//{
//    public class PlotPath
//    {
//        public static PlottedPath DoPlotPath(State state, Mat map, double scalling = 1)
//        {
//            var path = state.Path.T().ToMat();
//            path.Col(2).SetAll(1);
//            path.Col(3).SetAll(1);
//            Mat imag = state.Map;
            
//            Mat pathInterpolated;
//            if (scalling != 1)
//            {
//                path.Cols(1,1).SetAll(d => d - 2.5); ;
//                path.Cols(2,2).SetAll(d => d - 2.5);
//                pathInterpolated = BSpline.DoBSpline(path * scalling);
//            }
//            else
//            {

//                pathInterpolated = path;
//            }

//            foreach (var it in pathInterpolated.T().ToMat().AsMathlabColEnumerable())
//            {
//                var a = it.Round();
//                imag._Set<double>(a.__(1), a.__(2), 0.6);
//            }
//            var name = Path.GetTempFileName();
//            imag.SaveImage(name);
//            var retVal = new PlottedPath { Image = System.Drawing.Image.FromFile(name), Path = pathInterpolated };
//            File.Delete(name);
//            return retVal;
//        }
//    }
//}
