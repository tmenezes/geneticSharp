using System.Reflection;

namespace GeneticSharp.Mutation
{
    public class UniformMutation<T> : MutationBase<T> where T : class, new()
    {
        public UniformMutation(EvolutionOptions options) : base(options)
        {
        }

        protected override object MutateGene(PropertyInfo gene, int index, object originalValue, object suggestedRandomValue)
        {
            return suggestedRandomValue;
        }
    }
}