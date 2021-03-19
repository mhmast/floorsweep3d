using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FloorSweep.PathFinding
{
    internal class FocussedDStarSession : IPathFindingSession
    {
        private readonly MapData _data;
        
        public FocussedDStarSession(MapData data)
        {
            _data = data;
        }
        public Task<IPath> FindPathAsync(Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> debugCallback = null)
            => Task.Run(()=>FindPath(debugCallback));

        public IPath FindPath(Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> debugCallback = null)
        {
            var @bool = false;

            const string totalKey = "TOTAL";
            var statistics = new FocussedDStarStatistics(totalKey);
            var sw = Stopwatch.StartNew();

            var state = FDSInit.DoFDSInit(_data, _data.Scaling);
            debugCallback?.Invoke(
                new ReadOnlyDictionary<string, Mat>(state.ToDictionary(a => a.Item1, a => a.Item2)),
                new ReadOnlyDictionary<string, bool>(state.ToDictionary(a => a.Item1, a => a.Item3)));


            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            var total = ms;
            statistics.Add(nameof(FDSInit.DoFDSInit), ms);
            sw = Stopwatch.StartNew();

            state = FDSComputePath.DoFdsComputePath(state);

            sw.Stop();
            ms = sw.ElapsedMilliseconds;
            total += ms;
            statistics.Add(nameof(FDSComputePath.DoFdsComputePath), ms);

            @bool = true;
            var gah = state;
            state.KM = 50;

            if (@bool)
            {
                @bool = false;

                sw = Stopwatch.StartNew();

                state = FDSUpdateMap.DoFDSUpdateMap(state, _data.Map);

                sw.Stop();
                ms = sw.ElapsedMilliseconds;
                total += ms;
                statistics.Add(nameof(FDSUpdateMap.DoFDSUpdateMap), ms);

                state.KM = 50;
            }
            else
            {

                sw = Stopwatch.StartNew();

                state = FDSComputePath.DoFdsComputePath(state);

                sw.Stop();
                ms = sw.ElapsedMilliseconds;

                total += ms;
                statistics.Add(nameof(FDSComputePath.DoFdsComputePath), ms);

            }


            sw = Stopwatch.StartNew();

            ResolvePath.DoResolvePath(state);

            sw.Stop();
            ms = sw.ElapsedMilliseconds;
            total += ms;
            statistics.Add(nameof(ResolvePath.DoResolvePath), ms);
            statistics.Add(totalKey, total);
            return new FocussedDStarPath(state.Path, statistics);
        }
    }
}
