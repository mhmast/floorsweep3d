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
        private readonly Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> _debugDataRegistration;

        public FocussedDStarSession(MapData data, Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> debugDataRegistration)
        {
            _data = data;
            _debugDataRegistration = debugDataRegistration;
        }
        public Task<IPath> FindPathAsync()
            => Task.Run(FindPath);

        public IPath FindPath()
        {
            var @bool = false;
#if DEBUG
            var statistics = new Dictionary<string, long>();
            var sw = Stopwatch.StartNew();
#endif   
            var state = FDSInit.DoFDSInit(_data, _data.Scaling);
            _debugDataRegistration?.Invoke(
                new ReadOnlyDictionary<string, Mat>(state.ToDictionary(a => a.Key, a => a.Value)));

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
            return new FocussedDStarPath(state.Path, statistics);
#else
            return new FocussedDStarPath(state.Path);
#endif
        }
    }
}
