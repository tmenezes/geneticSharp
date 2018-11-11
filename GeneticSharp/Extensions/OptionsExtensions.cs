using AutoBuilder;

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
    }
}
