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
        {
            collection.AddTransient<IPathFindingAlgorithm,FocussedDStar>();
            return collection;
        }
    }
}
