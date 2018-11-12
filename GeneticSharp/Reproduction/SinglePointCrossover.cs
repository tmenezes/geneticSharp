using System.Linq;
using System.Reflection;

namespace GeneticSharp.Reproduction
{
    public class SinglePointCrossover<T> : PointedCrossover<T> where T : class, new()
    {
        public SinglePointCrossover(EvolutionOptions options) : base(options, cutPoints: 1)
        {
        }

        protected override T ChooseGeneGiver(PropertyInfo gene, int index, T parentA, T parentB)
        {
            var crossoverPoint = GetCrossoverPoint(parentA, parentB).First(); // it has just one crossover cut point
            return index <= crossoverPoint ? parentA : parentB;
        }
    }
}