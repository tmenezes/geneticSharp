using System.Collections.Generic;
using System.Linq;

namespace GeneticSharp.Tests.Reproduction
{
    public class CrossoverTestModel : IEvolutionaryIndividual
    {
        public List<int> Bits { get; set; }
        public decimal Fitness { get; }

        public CrossoverTestModel() { }
        public CrossoverTestModel(int[] bits) => Bits = bits.ToList();

        public void CalculateFitness()
        {
        }

        public override string ToString()
        {
            return Bits.Select(b => b.ToString()).Aggregate((one, two) => $"{one}{two}");
        }
    }
}