using System.Linq;
using System.Reflection;
using AutoBuilder;
using GeneticSharp.Helpers;

namespace GeneticSharp.Mutation
{
    public class RandomMutator<T> where T : class, new()
    {
        private readonly EvolutionOptions _options;
        private readonly Builder<T> _builder = new Builder<T>();

        public RandomMutator(EvolutionOptions options)
        {
            _options = options;
        }

        public void Mutate(T a)
        {
            var chromosomes = ReflectionHelper.GetProperties<T>().ToList();

            var chromosomesToChange = _options.MutationRate * chromosomes.Count;
            chromosomesToChange = chromosomesToChange <= 0 ? 1 : chromosomesToChange;

            for (int i = 0; i < chromosomesToChange; i++)
            {
                var chromosome = chromosomes[RandomData.GetInt(chromosomes.Count)];

                chromosome.SetValue(a, GetChromosomeNewValue(chromosome));

                chromosomes.Remove(chromosome);
            }
        }

        private object GetChromosomeNewValue(PropertyInfo chromosome)
        {
            var newGuy = _builder.Build();
            return chromosome.GetValue(newGuy);
        }
    }
}