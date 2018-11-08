using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class ActivateBitTest : UnitTestBase
    {
        public ActivateBitTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test1()
        {
            var options = new EvolutionOptions { CollectionTypesSizes = 8 };
            Evolve<ActivateBitModel>(options: options);
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
