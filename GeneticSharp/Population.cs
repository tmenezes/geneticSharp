using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Breeding;
using GeneticSharp.Mutation;

namespace GeneticSharp
{
    public class Population<T> : IEnumerable<T> where T : class, IEvolutionaryIndividual, new()
    {
        private readonly IEnumerable<T> _individuals;

        public Population(IEnumerable<T> individuals)
        {
            _individuals = individuals;
        }


        internal Population<T> ApplyNaturalSelection(EvolutionOptions options)
        {
            var naturalSelectedCount = (int)(options.GenerationQuantity * options.NaturalSelectionRate);

            _individuals.ToList().ForEach(i => i.CalculateFitness());

            var bestIndividuals = _individuals.OrderByDescending(i => i.Fitness);
            var naturalSelected = bestIndividuals.Take(naturalSelectedCount).ToList();

            return new Population<T>(naturalSelected);
        }

        internal Population<T> Breed(EvolutionOptions options)
        {
            return Breeder.Breed(this, options);
        }

        internal Population<T> Mutate(EvolutionOptions options)
        {
            return Mutator.Mutate(this, options);
        }


        public T GetBest() => _individuals.OrderByDescending(i => i.Fitness).First();
        public T GetWorst() => _individuals.OrderBy(i => i.Fitness).First();
        public T GetAverage() => _individuals.OrderBy(i => i.Fitness).ElementAt(_individuals.Count() / 2);

        public IEnumerator<T> GetEnumerator()
        {
            return _individuals.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}