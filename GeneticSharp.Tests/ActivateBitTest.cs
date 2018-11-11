using System.Linq;
using GeneticSharp.Reproduction;
using GeneticSharp.Selection;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class ActivateBitTest : UnitTestBase
    {
        private readonly ITestOutputHelper _output;

        public ActivateBitTest(ITestOutputHelper output) : base(output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            _output.WriteLine("Elite selection");
            var options1 = new EvolutionOptions { CollectionSize = 8 };
            Evolve<ActivateBitModel>(options: options1, generationsCount: 10);

            _output.WriteLine("\n-----------------------\n");
            _output.WriteLine("Proportional selection");
            var options2 = new EvolutionOptions { NaturalSelection = SelectionTypes.Proportional, CollectionSize = 8 };
            Evolve<ActivateBitModel>(options: options2, generationsCount: 10);

            _output.WriteLine("\n-----------------------\n");
            _output.WriteLine("Elite + Single-Point Crossover");
            var options3 = new EvolutionOptions { CollectionSize = 8, Crossover = CrossoverTypes.SinglePoint};
            Evolve<ActivateBitModel>(options: options3, generationsCount: 10);

            _output.WriteLine("\n-----------------------\n");
            _output.WriteLine("Elite + Slice Crossover");
            var options4 = new EvolutionOptions { CollectionSize = 8, Crossover = CrossoverTypes.Slice };
            Evolve<ActivateBitModel>(options: options4, generationsCount: 10);
        }
    }

    public class ActivateBitModel : IEvolutionaryIndividual
    {
        private decimal _fitness = 0;

        // demo of working with property or Collection (Arrays, ILists) property
        public bool Bit0 { get; set; }
        public bool Bit1 { get; set; }
        public bool[] Bit { get; set; }

        public decimal Fitness => _fitness;

        public void CalculateFitness()
        {
            _fitness += Bit0 ? 10 : 0;
            _fitness += Bit1 ? 10 : 0;
            _fitness += Bit.Count(i => i) * 10; // how many activated bits
        }

        public override string ToString()
        {
            string P(bool b) => b ? "1" : "0";

            return $"{nameof(Fitness)}: {Fitness}%: {P(Bit0)}{P(Bit1)}{Bit.Select(P).Aggregate((one, two) => $"{one}{two}")}";
        }
    }
}
