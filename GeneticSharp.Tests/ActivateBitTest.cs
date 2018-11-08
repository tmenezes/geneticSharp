using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class ActivateBitTest
    {
        private readonly ITestOutputHelper _output;

        public ActivateBitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var options = new EvolutionOptions() { CollectionTypesSizes = 8 };
            var geneticEvolution = new GeneticEvolution<ActivateBitModel>(options);

            for (int i = 0; i < 50; i++)
            {
                var result = geneticEvolution.Evolve();
                _output.WriteLine($"Gen. : #{geneticEvolution.CurrentGeneration.Number}");
                _output.WriteLine($"Best : {result.BestIndividual}");
                _output.WriteLine($"Avg  : {result.AverageIndividual}");
                _output.WriteLine($"Worst: {result.WorstIndividual}");
                _output.WriteLine($"------------------------------");
                _output.WriteLine($"Avg.Fitness: {result.AverageFitness}%");
                _output.WriteLine("");
            }
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
