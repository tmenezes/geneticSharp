using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoBuilder;
using GeneticSharp.Extensions;
using GeneticSharp.Helpers;

namespace GeneticSharp.Mutation
{
    public class RandomMutator<T> where T : class, new()
    {
        private readonly EvolutionOptions _options;
        private readonly Builder<T> _builder;

        public RandomMutator(EvolutionOptions options)
        {
            _options = options;
            _builder = options.GenerateBuilder<T>();
        }

        public void Mutate(T a)
        {
            var chromosomes = GetPropertiesWithCollectionsProportional();

            var chromosomesToChange = _options.MutationRate * chromosomes.Count;
            chromosomesToChange = chromosomesToChange <= 0 ? 1 : chromosomesToChange;

            for (int i = 0; i < chromosomesToChange; i++)
            {
                var chromosome = chromosomes[RandomData.GetInt(chromosomes.Count)];

                if (ReflectionHelper.IsCollection(chromosome.PropertyType))
                {
                    var collectionValue = chromosome.GetValue(a) as IList;
                    var newCollectionValue = GetChromosomeNewValue(chromosome) as IList;

                    var randomGenIndex = RandomData.GetInt(collectionValue.Count);
                    collectionValue[randomGenIndex] = newCollectionValue[randomGenIndex];
                }
                else
                {
                    chromosome.SetValue(a, GetChromosomeNewValue(chromosome));
                }

                chromosomes.Remove(chromosome);
            }
        }

        private List<PropertyInfo> GetPropertiesWithCollectionsProportional()
        {
            var props = ReflectionHelper.GetProperties<T>().ToList();
            foreach (var prop in props.ToList())
            {
                if (ReflectionHelper.IsCollection(prop.PropertyType))
                {
                    props.AddRange(Enumerable.Range(0, _options.CollectionTypesSizes - 1).Select(_ => prop));
                }
            }

            return props;
        }

        private object GetChromosomeNewValue(PropertyInfo chromosome)
        {
            var newGuy = _builder.Build();
            return chromosome.GetValue(newGuy);
        }
    }
}