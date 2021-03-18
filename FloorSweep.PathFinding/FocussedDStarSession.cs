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
#if DEBUG
            const string totalKey = "TOTAL";
            var statistics = new FocussedDStarStatistics(totalKey);
            var sw = Stopwatch.StartNew();
#endif   
            var state = FDSInit.DoFDSInit(_data, _data.Scaling);
            debugCallback?.Invoke(
                new ReadOnlyDictionary<string, Mat>(state.ToDictionary(a => a.Item1, a => a.Item2)),
                new ReadOnlyDictionary<string, bool>(state.ToDictionary(a => a.Item1, a => a.Item3)));

#if DEBUG
            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            var total = ms;
            statistics.Add(nameof(FDSInit.DoFDSInit), ms);
            sw = Stopwatch.StartNew();
#endif
            state = FDSComputePath.DoFdsComputePath(state);
#if DEBUG
            sw.Stop();
            ms = sw.ElapsedMilliseconds;
            total += ms;
            statistics.Add(nameof(FDSComputePath.DoFdsComputePath), ms);
#endif
            @bool = true;
            var gah = state;
            state.KM = 50;

            if (@bool)
            {
                @bool = false;
#if DEBUG
                sw = Stopwatch.StartNew();
#endif
                state = FDSUpdateMap.DoFDSUpdateMap(state, _data.Map);
#if DEBUG
                sw.Stop();
                ms = sw.ElapsedMilliseconds;
                total += ms;
                statistics.Add(nameof(FDSUpdateMap.DoFDSUpdateMap), ms);
#endif
                state.KM = 50;
            }
            else
            {
#if DEBUG
                sw = Stopwatch.StartNew();
#endif
                state = FDSComputePath.DoFdsComputePath(state);
#if DEBUG
                sw.Stop();
                ms = sw.ElapsedMilliseconds;

                total += ms;
                statistics.Add(nameof(FDSComputePath.DoFdsComputePath), ms);
#endif
            }

#if DEBUG
            sw = Stopwatch.StartNew();
#endif
            ResolvePath.DoResolvePath(state);
#if DEBUG
            sw.Stop();
            ms = sw.ElapsedMilliseconds;
            total += ms;
            statistics.Add(nameof(ResolvePath.DoResolvePath), ms);
            statistics.Add(totalKey, total);
            return new FocussedDStarPath(state.Path, statistics);
#else
            return new FocussedDStarPath(state.Path);
#endif
        }
    }
}
