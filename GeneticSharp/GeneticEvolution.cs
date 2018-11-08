using System.Collections.Generic;

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


        public EvolutionResult<T> Evolve()
        {
            SwitchGenerations();

            var naturalSelected = CurrentGeneration.Population.ApplyNaturalSelection(_options);

            // prepare next generation
            var nextPopulation = naturalSelected.Breed(_options).Mutate(_options);
            NextGeneration = new Generation<T>(CurrentGeneration.Number + 1, nextPopulation);

            return new EvolutionResult<T>(CurrentGeneration);
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
