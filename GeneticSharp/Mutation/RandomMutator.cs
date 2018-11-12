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
            var genes = GetPropertiesWithCollectionsProportional();

            var genesToChange = _options.MutationRate * genes.Count;
            genesToChange = genesToChange <= 0 ? 1 : genesToChange;

            for (int i = 0; i < genesToChange; i++)
            {
                var gene = genes[RandomData.GetInt(genes.Count)];

                if (ReflectionHelper.IsCollection(gene.PropertyType))
                {
                    var collectionValue = gene.GetValue(a) as IList;
                    var newCollectionValue = GetGeneNewValue(gene) as IList;

                    var randomGenIndex = RandomData.GetInt(collectionValue.Count);
                    collectionValue[randomGenIndex] = newCollectionValue[randomGenIndex];
                }
                else
                {
                    gene.SetValue(a, GetGeneNewValue(gene));
                }

                genes.Remove(gene);
            }
        }

        private List<PropertyInfo> GetPropertiesWithCollectionsProportional()
        {
            var props = ReflectionHelper.GetProperties<T>().ToList();
            foreach (var prop in props.ToList())
            {
                if (ReflectionHelper.IsCollection(prop.PropertyType))
                {
                    props.AddRange(Enumerable.Range(0, _options.CollectionSize - 1).Select(_ => prop));
                }
            }

            return props;
        }

        private object GetGeneNewValue(PropertyInfo gene)
        {
            var newGuy = _builder.Build();
            return gene.GetValue(newGuy);
        }
    }
}