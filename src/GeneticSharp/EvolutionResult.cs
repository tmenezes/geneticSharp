using System.Linq;

namespace GeneticSharp
{
    public class EvolutionResult<T> where T : class, IEvolutionaryIndividual, new()
    {
        public Generation<T> Generation { get; }
        public T BestIndividual { get; }
        public T AverageIndividual { get; }
        public T WorstIndividual { get; }
        public decimal AverageFitness { get; }

        public EvolutionResult(Generation<T> generation)
        {
            Generation = generation;
            BestIndividual = generation.Population.GetBest();
            AverageIndividual = generation.Population.GetAverage();
            WorstIndividual = generation.Population.GetWorst();
            AverageFitness = generation.Population.Average(i => i.Fitness);
        }
    }
}