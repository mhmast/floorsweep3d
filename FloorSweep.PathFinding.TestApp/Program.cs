using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloorSweep.PathFinding.TestApp
{
    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var mapsNames = new[] { "a", "e", "b", "c", "map01" };
            var maps = new List<MapData>();
            var scaling = 4;
            for (int i = 0; i < mapsNames.Length; i++)
            {
                var tmp = LoadMap.DoLoadMap(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, mapsNames[i] + ".png"), scaling);
                var start = tmp.Start.ComplexConjugate();
                var goal = tmp.Target.ComplexConjugate();
                maps.Add(new MapData { Map = tmp.Map, Start = start, Target = goal });
            }


            var @bool = false;
            var state = FDSInit.DoFDSInit(maps[0], scaling);
            state = FDSComputePath.DoFdsComputePath(state);
            @bool = true;
            var gah = state;
            state.KM = 50;

            if (@bool)
            {
                @bool = false;
                state = FDSUpdateMap.DoFDSUpdateMap(state, maps[0].Map);
                state.KM = 50;
            }
            else
            {
                state = FDSComputePath.DoFdsComputePath(state);
            }

            //%% resolve path, insert map name here if you want to get image in original size when map was downscalled
            state.Path = ResolvePath.DoResolvePath(state);
            var resp = PlotPath.DoPlotPath(state, "e", scaling );

            //%% show A - star graph
            //  a = state.graph(:,:,1);
            //            a(a(:,:) == -1) = 0; % unvisited
            //a(state.map(:,:) == 0) = 0.2; % obstacles
            //a(a(:,:) == inf) = 0;
            //            a(a(:,:) >= 1) = 0.3;
            //            imshow(a)
            //            %% show D - star graph
            //              % figure
            //              % obstacles 0.2
            //              % visited : 0.3
            //              % unavailble, unvisited: 0
            //              % in queue: 1
            //              % in queue and g - val == inf: 0.7
            //a = state.graph(:,:, 1);
            //            a(a(:,:)~= inf) = 0.3;
            //            a(a(:,:) == inf) = 0;
            //            a(state.map(:,:) == 0) = 0.2;
            //            b = state.graph(:,:, 3);
            //            b(state.map(:,:) == 0) = 0;
            //            b(state.graph(:,:, 2) == inf) = 0;
            //            b = b * 0.7;
            //            imshow(b + a, 'Border', 'tight')








            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(resp));
        }
    }
}
