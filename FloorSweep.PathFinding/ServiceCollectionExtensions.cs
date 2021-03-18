using FloorSweep.Math;
using FloorSweep.PathFinding.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FloorSweep.PathFinding
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseFocussedDStar(this IServiceCollection collection)
            => collection.UseFocussedDStar(_ => { });
        public static IServiceCollection UseFocussedDStar(this IServiceCollection collection, Action<IReadOnlyDictionary<string, Mat>> debugDataRegistration)
        {
            collection.AddTransient<IPathFindingAlgorithm>(s => new FocussedDStar(debugDataRegistration));
            return collection;
        }
    }
}
