using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoBuilder;
using AutoBuilder.Helpers;
using GeneticSharp.Extensions;
using GeneticSharp.Helpers;

namespace GeneticSharp.Mutation
{
    public enum MutationTypes
    {
        /// <summary>
        /// Performs mutation by randomly generating new gene value
        /// </summary>
        Uniform,

        /// <summary>
        /// Add or subtract a small number from the current gene value
        /// </summary>
        Addition,
    }

    public abstract class MutationBase<T> where T : class, new()
    {
        private readonly EvolutionOptions _options;
        protected readonly Builder<T> Builder;

        protected MutationBase(EvolutionOptions options)
        {
            _options = options;
            Builder = options.GenerateBuilder<T>();
        }


        public void Mutate(T a)
        {
            var genes = GetGenesNormalizedSizes();

            var genesToChange = _options.MutationRate * genes.Count;
            genesToChange = genesToChange < 1 ? 1 : genesToChange;

            for (int i = 0; i < genesToChange; i++)
            {
                var gene = genes[RandomData.GetInt(genes.Count)];

                if (ReflectionHelper.IsCollection(gene.PropertyType))
                {
                    var collectionValue = gene.GetValue(a) as IList;
                    var newCollection = GetNewValueRandomly(gene) as IList;

                    var randomGeneIndex = RandomData.GetInt(collectionValue.Count);
                    var newValue = MutateGene(gene, i + randomGeneIndex, collectionValue[randomGeneIndex], newCollection[randomGeneIndex]);

                    Mutator.SetOrFallback(
                        () => collectionValue[randomGeneIndex] = newValue,
                        () => collectionValue[randomGeneIndex] = newCollection[randomGeneIndex]
                    );
                }
                else
                {
                    var suggestValue = GetNewValueRandomly(gene);
                    var newValue = MutateGene(gene, i, gene.GetValue(a), suggestValue);

                    Mutator.SetOrFallback(
                        () => gene.SetValue(a, newValue),
                        () => gene.SetValue(a, suggestValue)
                    );
                }

                genes.Remove(gene);
            }
        }

        protected abstract object MutateGene(PropertyInfo gene, int index, object originalValue, object suggestedRandomValue);


        protected object GetNewValueRandomly(PropertyInfo gene)
        {
            var newGuy = Builder.Build();
            return gene.GetValue(newGuy);
        }

        private List<PropertyInfo> GetGenesNormalizedSizes()
        {
            var props = ReflectionHelper.GetProperties<T>();
            var result = props.ToList();

            foreach (var prop in props)
            {
                if (ReflectionHelper.IsCollection(prop.PropertyType))
                {
                    result.AddRange(Enumerable.Range(0, _options.CollectionSize - 1).Select(_ => prop));
                }
            }

            return result;
        }
    }
}