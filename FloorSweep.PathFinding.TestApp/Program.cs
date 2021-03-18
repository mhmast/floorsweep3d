
using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            int mapno = 1;
            var maps = new List<MapData>();
            var scaling = 4;
            for (int i = 0; i < mapsNames.Length; i++)
            {
                var path = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, mapsNames[i] + ".png");
                var tmp = MapData.FromImage(path, new Point(74, 86), new Point(541, 469), scaling);
                maps.Add(tmp);
            }
           
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
           // var state = FDSInit.DoFDSInit(maps[mapno], scaling);

            Application.Run(new Form1());






        }
    }
}
