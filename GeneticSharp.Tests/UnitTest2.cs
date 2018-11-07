using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class UnitTest2
    {
        private readonly ITestOutputHelper _output;

        public UnitTest2(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var geneticEvolution = new GeneticEvolution<ActivateBitModel>();
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

    public class ActivateBitModel : IEvolutionaryIndividual
    {
        private decimal _fitness = 0;

        public bool Bit0 { get; set; }
        public bool Bit1 { get; set; }
        public bool Bit2 { get; set; }
        public bool Bit3 { get; set; }
        public bool Bit4 { get; set; }
        public bool Bit5 { get; set; }
        public bool Bit6 { get; set; }
        public bool Bit7 { get; set; }
        public bool Bit8 { get; set; }
        public bool Bit9 { get; set; }


        public decimal Fitness => _fitness;

        public void CalculateFitness()
        {
            _fitness += Bit0 ? 10 : 0;
            _fitness += Bit1 ? 10 : 0;
            _fitness += Bit2 ? 10 : 0;
            _fitness += Bit3 ? 10 : 0;
            _fitness += Bit4 ? 10 : 0;
            _fitness += Bit5 ? 10 : 0;
            _fitness += Bit6 ? 10 : 0;
            _fitness += Bit7 ? 10 : 0;
            _fitness += Bit8 ? 10 : 0;
            _fitness += Bit9 ? 10 : 0;
        }

        public override string ToString()
        {
            char P(bool b) => b ? '1' : '0';

            return $"{nameof(Fitness)}: {Fitness}%: {P(Bit0)}{P(Bit1)}{P(Bit2)}{P(Bit3)}{P(Bit4)}{P(Bit5)}{P(Bit6)}{P(Bit7)}{P(Bit8)}{P(Bit9)}";
        }
    }
}
