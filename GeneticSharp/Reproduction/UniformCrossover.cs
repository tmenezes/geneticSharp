using System.Reflection;
using AutoBuilder.Helpers;

namespace GeneticSharp.Reproduction
{
    public class UniformCrossover<T> : CrossoverBase<T> where T : class, new()
    {
        public UniformCrossover(EvolutionOptions options) : base(options)
        {
        }

        protected override T ChooseChromosomeGiver(PropertyInfo chromosome, int index, T parentA, T parentB)
        {
            return RandomData.GetBool() ? parentA : parentB;
        }
    }
}
