using System.Linq;

namespace GeneticSharp.Selection
{
    public class EliteSelection<T> : INaturalSelection<T> where T : class, IEvolutionaryIndividual, new()
    {
        private readonly double _naturalSelectionRate;

        public EliteSelection(double naturalSelectionRate = ConstantValues.NaturalSelectionRate)
        {
            _naturalSelectionRate = naturalSelectionRate;
        }

        public Population<T> Select(Population<T> population)
        {
            var naturalSelectedCount = (int)(population.Count() * _naturalSelectionRate);

            var bestIndividuals = population.OrderByDescending(i => i.Fitness)
                                            .Take(naturalSelectedCount)
                                            .ToList();

            return new Population<T>(bestIndividuals);
        }
    }
}
