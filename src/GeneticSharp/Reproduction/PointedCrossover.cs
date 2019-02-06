using System;
using System.Collections.Concurrent;
using System.Linq;
using AutoBuilder.Helpers;
using GeneticSharp.Helpers;

namespace GeneticSharp.Reproduction
{
    public abstract class PointedCrossover<T> : CrossoverBase<T> where T : class, new()
    {
        private readonly ConcurrentDictionary<int, int[]> _parentsCrossoverPoint = new ConcurrentDictionary<int, int[]>();

        protected readonly int CutPoints;
        protected readonly int GenesCount;

        protected PointedCrossover(EvolutionOptions options, int cutPoints = 1) : base(options)
        {
            CutPoints = cutPoints;
            GenesCount = CalculateGenesCount(options.CollectionSize);

            if (GenesCount <= cutPoints)
                throw new InvalidOperationException($"It is not possible to split the current model data using {cutPoints} cut points.");
        }

        // helpers & private
        protected int[] GetCrossoverPoint(T a, T b)
        {
            var parentsHash = GetParentHash(a, b);
            return _parentsCrossoverPoint.GetOrAdd(parentsHash, hash =>
            {
                var points = new int[CutPoints];
                var previousPoint = -1;
                for (int i = 0; i < CutPoints; i++)
                {
                    var otherPointsCount = CutPoints - (i + 1);
                    points[i] = RandomData.GetInt(previousPoint + 1, GenesCount - otherPointsCount - 2);
                    previousPoint = points[i];
                }

                return points;
            });
        }

        private static int GetParentHash(T a, T b)
        {
            var hash = 23;
            hash = hash * 31 + a.GetHashCode();
            hash = hash * 31 + b.GetHashCode();
            return hash;
        }

        private static int CalculateGenesCount(int collectionSize)
        {
            var properties = ReflectionHelper.GetProperties<T>();
            var collectionCount = properties.Count(ReflectionHelper.IsCollection);
            var regularPropsCount = properties.Count() - collectionCount;

            return regularPropsCount + collectionCount * collectionSize;
        }
    }
}