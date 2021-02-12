using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloorSweep.PathFinding
{
    public class PlotPath
    {
        public static object PlotPath(State state, string mapName, double scalling = 1)
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
                var tmp = LoadMap.DoLoadMap($"{mapName}.png", 1);
                imag = tmp.Map;
            }

            Mat pathInterpolated;
            if (scalling != 1) {
                path.Col(0).SetAll(d => d - 2.5); ;
                path.Col(1).SetAll(d => d - 2.5);
                pathInterpolated = BSpline(path * scalling);
            }
            else {

                pathInterpolated = path;
            }
            %% 5
    for it = pathInterpolated'
        a = round(it);
        imag(a(1), a(2)) = 0.6;
    end
    figure(100)
    out.handle = imshow(imag);
    out.path = pathInterpolated;

            end
    }
    }
