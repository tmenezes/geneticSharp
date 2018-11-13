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
        /// Performs crossover by randomly selecting each gene from the parent A or B
        /// </summary>
        Uniform,

        /// <summary>
        /// Defines one cut point between the number of genes and pickup one side from parent A, then the other from parent B
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
            var genes = ReflectionHelper.GetProperties<T>().ToList();

            for (var i = 0; i < genes.Count; i++)
            {
                var gene = genes[i];
                var value = default(object);

                if (ReflectionHelper.IsCollection(gene.PropertyType))
                {
                    value = SetGeneCollectionValue(gene, i, a, b);
                }
                else
                {
                    var giver = ChooseGeneGiver(gene, i, a, b);
                    value = gene.GetValue(giver);
                }

                gene.SetValue(newIndividual, value);
            }

            return newIndividual;
        }

        // customizations
        protected abstract T ChooseGeneGiver(PropertyInfo gene, int index, T parentA, T parentB);

        // helpers & privates
        protected object GetGeneNewValue(PropertyInfo gene)
        {
            var newGuy = Builder.Build();
            return gene.GetValue(newGuy);
        }

        private object SetGeneCollectionValue(PropertyInfo gene, int index, T parentA, T parentB)
        {
            var collectionValue = GetGeneNewValue(gene) as IList;
            var valueParentA = gene.GetValue(parentA) as IList;
            var valueParentB = gene.GetValue(parentB) as IList;

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var parent = ChooseGeneGiver(gene, index + i, parentA, parentB);
                var giver = parent == parentA ? valueParentA : valueParentB;

                collectionValue[i] = giver[i];
            }

            return collectionValue;
        }
    }
}