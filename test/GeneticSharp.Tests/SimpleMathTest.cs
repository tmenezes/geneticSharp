using System;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class SimpleMathTest : UnitTestBase
    {
        public SimpleMathTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test1()
        {
            Evolve<SimpleMathModel>();
        }
    }

    public class SimpleMathModel : IEvolutionaryIndividual
    {
        private decimal _fitness = 0;
        public enum SimpleMathOperation
        {
            Sum, Subtract, Divide, Multiply
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
                case SimpleMathOperation.Divide: result = n1 / (n2 == 0 ? 1 : n2); break;
                case SimpleMathOperation.Multiply: result = n1 * n2; break;
            }

            var expectedResult = (long)Number1 + Number2;
            var diff = Math.Abs(expectedResult - result);

            var adjustedValue = int.MaxValue - diff;
            _fitness = adjustedValue * 100 / int.MaxValue;
        }

        public override string ToString()
        {
            return $"{nameof(Fitness)}: {Fitness}%: N1: {Number1}-{KnowsNumber1}, N2: {Number2}-{KnowsNumber2}, Op.: {Operation}";
        }
    }
}
