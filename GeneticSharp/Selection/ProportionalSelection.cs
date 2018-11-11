using System.Linq;
using GeneticSharp.Extensions;

namespace GeneticSharp.Selection
{
    public class ProportionalSelection<T> : INaturalSelection<T> where T : class, IEvolutionaryIndividual, new()
    {
        private readonly double _naturalSelectionRate;

        public ProportionalSelection(double naturalSelectionRate = ConstantValues.NaturalSelectionRate)
        {
            _naturalSelectionRate = naturalSelectionRate;
        }

        public Population<T> Select(Population<T> population)
        {
            var naturalSelectedCount = (int)(population.Count() * _naturalSelectionRate);

            var bestIndividuals = Enumerable.Range(0, naturalSelectedCount)
                                            .Select(_ => population.SelectProportional(i => (int)i.Fitness))
                                            .ToList();

            return new Population<T>(bestIndividuals);
        }
    }
}