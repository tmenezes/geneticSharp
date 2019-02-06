using System;
using System.Collections.Generic;
using GeneticSharp.Extensions;
using GeneticSharp.Mutation;
using GeneticSharp.Reproduction;

namespace GeneticSharp
{
    public delegate void IndividualEvolutionEventHandler<T>(T individual, Generation<T> generation) where T : class, IEvolutionaryIndividual, new();
    public delegate void GenerationEvolutionEventHandler<T>(EvolutionResult<T> generationResult) where T : class, IEvolutionaryIndividual, new();

    public class GeneticEvolution<T> where T : class, IEvolutionaryIndividual, new()
    {
        private readonly EvolutionOptions _options;
        private readonly List<Generation<T>> _generations = new List<Generation<T>>();

        public event IndividualEvolutionEventHandler<T> BeforeNaturalSelection;
        public event IndividualEvolutionEventHandler<T> AfterNaturalSelection;
        public event GenerationEvolutionEventHandler<T> GenerationEvolved;

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
            var naturalSelected = Select(CurrentGeneration);
            var newIndividuals = Reproduce(naturalSelected);
            var nextPopulation = Mutate(newIndividuals);

            // prepare next generation
            NextGeneration = new Generation<T>(CurrentGeneration.Number + 1, nextPopulation);

            // result of current generation
            var result = new EvolutionResult<T>(CurrentGeneration);
            GenerationEvolved?.Invoke(result);

            return result;
        }

        public IEnumerable<EvolutionResult<T>> EvolveUntil(Func<EvolutionResult<T>, bool> stopCondition, Func<EvolutionResult<T>, bool> onGenerationProcessed = null)
        {
            var allResults = new List<EvolutionResult<T>>();
            var result = default(EvolutionResult<T>);

            do
            {
                result = Evolve();
                allResults.Add(result);

                var shouldContinue = onGenerationProcessed?.Invoke(result) ?? true;
                if (!shouldContinue)
                    break;
            }
            while (!stopCondition.Invoke(result));

            return allResults;
        }

        // privates
        private Population<T> Select(Generation<T> generation)
        {
            foreach (var individual in generation.Population)
            {
                BeforeNaturalSelection?.Invoke(individual, generation);

                individual.CalculateFitness();

                AfterNaturalSelection?.Invoke(individual, generation);
            }

            return _options.GetSelection<T>()
                           .Select(generation.Population);
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
