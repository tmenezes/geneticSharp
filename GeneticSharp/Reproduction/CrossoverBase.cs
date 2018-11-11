using System.Collections;
using System.Linq;
using System.Reflection;
using AutoBuilder;
using GeneticSharp.Extensions;
using GeneticSharp.Helpers;

namespace GeneticSharp.Reproduction
{
    public enum CrossoverTypes
    {
        /// <summary>
        /// Performs crossover by randomly selecting each chromosome from the parent A or B
        /// </summary>
        Uniform,

        /// <summary>
        /// Defines one cut point between the number of chromosomes and pickup one side from parent A, then the other from parent B
        /// </summary>
        SinglePoint,

        /// <summary>
        /// Defines two cut points representing a DNA slice of parent B, then uses everything else from parent A
        /// </summary>
        Slice
    }

    public abstract class CrossoverBase<T> where T : class, new()
    {
        protected readonly Builder<T> Builder;

        protected CrossoverBase(EvolutionOptions options)
        {
            Builder = options.GenerateBuilder<T>();

        }

        // main method
        public virtual T Produce(T a, T b)
        {
            var newIndividual = new T();
            var chromosomes = ReflectionHelper.GetProperties<T>().ToList();

            for (var i = 0; i < chromosomes.Count; i++)
            {
                var chromosome = chromosomes[i];
                var value = default(object);

                if (ReflectionHelper.IsCollection(chromosome.PropertyType))
                {
                    value = SetChromosomeCollectionValue(chromosome, i, a, b);
                }
                else
                {
                    var giver = ChooseChromosomeGiver(chromosome, i, a, b);
                    value = chromosome.GetValue(giver);
                }

                chromosome.SetValue(newIndividual, value);
            }

            return newIndividual;
        }

        // customizations
        protected abstract T ChooseChromosomeGiver(PropertyInfo chromosome, int index, T parentA, T parentB);

        // helpers & privates
        protected object GetChromosomeNewValue(PropertyInfo chromosome)
        {
            var newGuy = Builder.Build();
            return chromosome.GetValue(newGuy);
        }

        private object SetChromosomeCollectionValue(PropertyInfo chromosome, int index, T parentA, T parentB)
        {
            var collectionValue = GetChromosomeNewValue(chromosome) as IList;
            var valueParentA = chromosome.GetValue(parentA) as IList;
            var valueParentB = chromosome.GetValue(parentB) as IList;

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var parent = ChooseChromosomeGiver(chromosome, index + i, parentA, parentB);
                var giver = parent == parentA ? valueParentA : valueParentB;

                collectionValue[i] = giver[i];
            }

            return collectionValue;
        }
    }
}