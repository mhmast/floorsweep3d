using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using FloorSweep.PathFinding;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Floorsweep.PathFinding.Tests
{
  
    [TestFixture]
    public class PathFindingBenchmark
    {

        List<MapData> _maps = new List<MapData>();

        [Params(1, 4)]
        public int scaling;
        [Params(0, 1, 2, 3, 4)]
        public int mapNo;

        [GlobalSetup]
        public void Setup()
        {
             var mapsNames = new[] { "a", "e", "b", "c", "map01" };
            for (int i = 0; i < mapsNames.Length; i++)
            {
                var tmp = LoadMap.DoLoadMap(Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, mapsNames[i] + ".png"), scaling);
                _maps.Add(tmp);
            }
        }
        [Benchmark(Baseline =true)]
        public void FindPath()
        {
            var @bool = false;
            var state = FDSInit.DoFDSInit(_maps[mapNo], scaling);

            state = FDSComputePath.DoFdsComputePath(state);
            @bool = true;
            var gah = state;
            state.KM = 50;

            if (@bool)
            {
                @bool = false;
                state = FDSUpdateMap.DoFDSUpdateMap(state, _maps[mapNo].Map);
                state.KM = 50;
            }
            else
            {
                state = FDSComputePath.DoFdsComputePath(state);
            }

            //%% resolve path, insert map name here if you want to get image in original size when map was downscalled
            ResolvePath.DoResolvePath(state);
        }
        
        
        public void Benchmark()
        {
            var sum = BenchmarkRunner.Run<PathFindingBenchmark>();
            Process.Start("explorer",Path.Combine(sum.ResultsDirectoryPath, "Floorsweep.PathFinding.Tests.PathFindingBenchmark-report.html"));
        }

    }
}