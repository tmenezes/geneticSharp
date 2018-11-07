using System.Linq;
using AutoBuilder;

namespace GeneticSharp
{
    public class Generation<T> where T : class, IEvolutionaryIndividual, new()
    {
        public int Number { get; }
        public Population<T> Population { get; }

        public Generation(int number, Population<T> population)
        {
            Number = number;
            Population = population;
        }


        internal static Generation<T> GenerateRandomly(EvolutionOptions options)
        {
            var builder = new Builder<T>();
            var individuals = Enumerable.Range(0, options.GenerationQuantity).Select(_ => builder.Build()).ToList();

            return new Generation<T>(1, new Population<T>(individuals));
        }
    }
}