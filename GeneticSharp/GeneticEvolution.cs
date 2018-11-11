using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Mutation;
using GeneticSharp.Reproduction;
using GeneticSharp.Selection;

namespace GeneticSharp
{
    public class GeneticEvolution<T> where T : class, IEvolutionaryIndividual, new()
    {
        private readonly EvolutionOptions _options;
        private readonly List<Generation<T>> _generations = new List<Generation<T>>();

        public IEnumerable<Generation<T>> Generations => _generations;
        public Generation<T> CurrentGeneration { get; private set; }
        public Generation<T> NextGeneration { get; private set; }

        // constructors
        public GeneticEvolution() : this(EvolutionOptions.Default)
        {
        }

        public GeneticEvolution(EvolutionOptions options)
        {
            _options = options;

            CurrentGeneration = Generation<T>.GenerateRandomly(_options);
            _generations.Add(CurrentGeneration);
        }

        // main method
        public EvolutionResult<T> Evolve()
        {
            SwitchGenerations();

            // perform evolution
            var naturalSelected = Select(CurrentGeneration.Population);
            var newIndividuals = Reproduce(naturalSelected);
            var nextPopulation = Mutate(newIndividuals);

            // prepare next generation
            NextGeneration = new Generation<T>(CurrentGeneration.Number + 1, nextPopulation);

            // result of current generation
            return new EvolutionResult<T>(CurrentGeneration);
        }

        // privates
        private Population<T> Select(Population<T> population)
        {
            population.ToList().ForEach(i => i.CalculateFitness());

            var selector = _options.NaturalSelection == SelectionTypes.Elite
                    ? new EliteSelection<T>(_options.NaturalSelectionRate)
                    : new ProportionalSelection<T>(_options.NaturalSelectionRate) as INaturalSelection<T>;

            return selector.Select(population);
        }

        private Population<T> Reproduce(Population<T> population)
        {
            return Breeder.Breed(population, _options);
        }

        private Population<T> Mutate(Population<T> population)
        {
            return Mutator.Mutate(population, _options);
        }

        private void SwitchGenerations()
        {
            if (NextGeneration == null)
                return;

            CurrentGeneration = NextGeneration;
            _generations.Add(CurrentGeneration);
            NextGeneration = null;
        }
    }
}
