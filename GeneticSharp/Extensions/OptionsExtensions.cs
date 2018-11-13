using AutoBuilder;
using GeneticSharp.Mutation;
using GeneticSharp.Reproduction;
using GeneticSharp.Selection;

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

        internal static INaturalSelection<T> GetSelection<T>(this EvolutionOptions options) where T : class, IEvolutionaryIndividual, new()
        {
            switch (options.NaturalSelection)
            {
                case SelectionTypes.Proportional:
                    return new ProportionalSelection<T>(options.NaturalSelectionRate);

                case SelectionTypes.Truncate:
                default:
                    return new TruncateSelection<T>(options.NaturalSelectionRate);
            }
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

        internal static MutationBase<T> GetMutation<T>(this EvolutionOptions options) where T : class, new()
        {
            switch (options.Mutation)
            {
                case MutationTypes.Addition:
                    return new AdditionMutation<T>(options);

                case MutationTypes.Uniform:
                default:
                    return new UniformMutation<T>(options);
            }
        }

        internal static bool IsInRange(this EvolutionOptions options, decimal value)
        {
            return value >= options.MinNumberValue && value <= options.MaxNumberValue;
        }
    }
}
