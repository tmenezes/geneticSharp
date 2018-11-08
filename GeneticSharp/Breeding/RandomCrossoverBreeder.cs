using System.Collections;
using System.Reflection;
using AutoBuilder;
using GeneticSharp.Extensions;
using GeneticSharp.Helpers;

namespace GeneticSharp.Breeding
{
    public class RandomCrossoverBreeder<T> where T : class, new()
    {
        private readonly Builder<T> _builder;

        public RandomCrossoverBreeder(EvolutionOptions options)
        {
            _builder = options.GenerateBuilder<T>();

        }

        public T Breed(T a, T b)
        {
            var newIndividual = new T();
            var chromosomes = ReflectionHelper.GetProperties<T>();

            foreach (var chromosome in chromosomes)
            {
                if (ReflectionHelper.IsCollection(chromosome.PropertyType))
                {
                    SetChromosomeCollectionValue(newIndividual, chromosome, a, b);
                }
                else
                {
                    var giver = RandomData.GetBool() ? a : b;
                    var value = chromosome.GetValue(giver);

                    chromosome.SetValue(newIndividual, value);
                }
            }

            return newIndividual;
        }


        private void SetChromosomeCollectionValue(T newIndividual, PropertyInfo chromosome, T a, T b)
        {
            var collectionValue = GetChromosomeNewValue(chromosome) as IList;
            var valueFather = chromosome.GetValue(a) as IList;
            var valueMother = chromosome.GetValue(b) as IList;

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var giver = RandomData.GetBool() ? valueFather : valueMother;
                collectionValue[i] = giver[i];
            }

            chromosome.SetValue(newIndividual, collectionValue);
        }

        private object GetChromosomeNewValue(PropertyInfo chromosome)
        {
            var newGuy = _builder.Build();
            return chromosome.GetValue(newGuy);
        }
    }
}
