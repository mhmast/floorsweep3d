using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class PlotPath
    {
        public static PlottedPath DoPlotPath(State state, string mapName, double scalling = 1)
        {
            var path = state.Path.T().ToMat();
            path.Col(2).SetAll(1);
            path.Col(3).SetAll(1);
            Mat imag;
            if (scalling == 1)
            {
                scalling = 1;
                imag = state.Map;
            }
            else
            {
                var tmp = LoadMap.DoLoadMap(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName,$"{mapName}.png"), 1);
                imag = tmp.Map;
            }

            Mat pathInterpolated;
            if (scalling != 1)
            {
                path.Col(0).SetAll(d => d - 2.5); ;
                path.Col(1).SetAll(d => d - 2.5);
                pathInterpolated = BSpline.DoBSpline(path * scalling);
            }
            else
            {

                pathInterpolated = path;
            }

            foreach (var it in pathInterpolated.T().ToMat().AsMathlabColEnumerable())
            {
                var a = it.Round().T().ToMat();
                imag.Set(a.Get<int>(0), a.Get<int>(1), 0.6);
            }
            var name = Path.GetTempFileName();
            imag.SaveImage(name);
            var retVal = new PlottedPath { Image = System.Drawing.Image.FromFile(name), Path = pathInterpolated };
            File.Delete(name);
            return retVal;
        }
    }
}
