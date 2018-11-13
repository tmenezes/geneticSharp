using AutoBuilder;
using GeneticSharp.Reproduction;

namespace GeneticSharp.Extensions
{
    public static class OptionsExtensions
    {
        internal static Builder<T> GenerateBuilder<T>(this EvolutionOptions options) where T : class, new()
        {
            return new Builder<T>().WithCollectionDegree(options.CollectionSize)
                                   .WithMinNumberValueOf(options.MinNumberValue)
                                   .WithMaxNumberValueOf(options.MaxNumberValue);
        }

        internal static CrossoverBase<T> GetCrossover<T>(this EvolutionOptions options) where T : class, new()
        {
            switch (options.Crossover)
            {
                case CrossoverTypes.SinglePoint:
                    return new SinglePointCrossover<T>(options);

                case CrossoverTypes.Slice:
                    return new SliceCrossover<T>(options);

                case CrossoverTypes.Uniform:
                default:
                    return new UniformCrossover<T>(options);
            }
        }

        internal static bool IsInRange(this EvolutionOptions options, decimal value)
        {
            return value >= options.MinNumberValue && value <= options.MaxNumberValue;
        }
    }
}
