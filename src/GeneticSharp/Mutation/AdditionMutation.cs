using System.Reflection;
using AutoBuilder.Helpers;
using GeneticSharp.Extensions;
using GeneticSharp.Helpers;

namespace GeneticSharp.Mutation
{
    public class AdditionMutation<T> : MutationBase<T> where T : class, new()
    {
        const decimal ADD_RATE = 0.1m;
        private readonly EvolutionOptions _options;

        public AdditionMutation(EvolutionOptions options) : base(options)
        {
            _options = options;
        }

        protected override object MutateGene(PropertyInfo gene, int index, object originalValue, object suggestedRandomValue)
        {
            if (NumberHelper.TryGetValue(originalValue, out var value))
            {
                var addValue = (value * ADD_RATE) * (decimal)RandomData.GetDouble(); // something around 10% (ADD_RATE), then a random fraction of it
                var shouldAdd = RandomData.GetBool(); // add or subtract

                var result = shouldAdd
                    ? value + addValue
                    : value - addValue;

                return _options.IsInRange(result)
                    ? NumberHelper.CastValueToProperType(gene.PropertyType, result)
                    : suggestedRandomValue;
            }
            else
            {
                return suggestedRandomValue;
            }
        }
    }
}