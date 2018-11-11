using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Mutation;
using GeneticSharp.Reproduction;
using GeneticSharp.Selection;

namespace GeneticSharp
{
    public class Population<T> : IEnumerable<T> where T : class, IEvolutionaryIndividual, new()
    {
        private readonly IEnumerable<T> _individuals;

        public Population(IEnumerable<T> individuals)
        {
            _individuals = individuals;
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