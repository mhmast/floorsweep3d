using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    internal class FocussedDStar : IPathFindingAlgorithm
    {
        private readonly Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> _debugDataRegistration;

        public FocussedDStar(Action<IReadOnlyDictionary<string, Mat>, IReadOnlyDictionary<string, bool>> debugDataRegistration)
        {
            _debugDataRegistration = debugDataRegistration;
        }

        public IPathFindingSession CreateSession(MapData data) => new FocussedDStarSession(data, _debugDataRegistration);
    }
}
