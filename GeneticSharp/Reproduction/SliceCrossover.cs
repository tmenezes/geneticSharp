using System.Reflection;

namespace GeneticSharp.Reproduction
{
    public class SliceCrossover<T> : PointedCrossover<T> where T : class, new()
    {
        public SliceCrossover(EvolutionOptions options) : base(options, cutPoints: 2)
        {
        }

        protected override T ChooseGeneGiver(PropertyInfo gene, int index, T parentA, T parentB)
        {
            var crossoverPoints = GetCrossoverPoint(parentA, parentB);
            return index <= crossoverPoints[0] || index > crossoverPoints[1] ? parentA : parentB;
        }
    }
}