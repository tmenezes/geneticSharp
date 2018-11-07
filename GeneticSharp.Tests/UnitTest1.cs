using System;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var geneticEvolution = new GeneticEvolution<SimpleMathModel>();
            for (int i = 0; i < 50; i++)
            {
                var result = geneticEvolution.Evolve();
                _output.WriteLine($"Gen. : #{geneticEvolution.CurrentGeneration.Number}");
                _output.WriteLine($"Best : {result.BestIndividual}");
                _output.WriteLine($"Avg  : {result.AverageIndividual}");
                _output.WriteLine($"Worst: {result.WorstIndividual}");
                _output.WriteLine($"------------------------------");
                _output.WriteLine($"Avg.Fitnes: {result.AverageFitness}%");
                _output.WriteLine("");
            }
        }
    }

    public class SimpleMathModel : IEvolutionaryIndividual
    {
        private decimal _fitness = 0;
        public enum SimpleMathOperation
        {
            Sum, Subtract, Divid, Multiply
        }

        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public SimpleMathOperation Operation { get; set; }
        public bool KnowsNumber1 { get; set; }
        public bool KnowsNumber2 { get; set; }


        public decimal Fitness => _fitness;

        public void CalculateFitness()
        {
            var n1 = KnowsNumber1 ? Number1 : 0;
            var n2 = KnowsNumber2 ? Number2 : 0;
            var result = 0;
            switch (Operation)
            {
                case SimpleMathOperation.Sum: result = n1 + n2; break;
                case SimpleMathOperation.Subtract: result = n1 - n2; break;
                case SimpleMathOperation.Divid: result = n1 / (n2 == 0 ? 1 : n2); break;
                case SimpleMathOperation.Multiply: result = n1 * n2; break;
            }

            _fitness += Operation == SimpleMathOperation.Sum ? 30 : 0;
            _fitness += KnowsNumber1 ? 20 : 0;
            _fitness += KnowsNumber2 ? 20 : 0;
            _fitness += Number1 < 100 ? 10 : 0;
            _fitness += Number2 < 100 ? 10 : 0;
            _fitness += result < 1000 ? result < 500 ? 10 : 5 : 0;
        }

        public override string ToString()
        {
            return $"{nameof(Fitness)}: {Fitness}%: N1: {Number1}-{KnowsNumber1}, N2: {Number2}-{KnowsNumber2}, Op.: {Operation}";
        }
    }
}
