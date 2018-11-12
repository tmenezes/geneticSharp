using System;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class UnitTestBase
    {
        private readonly ITestOutputHelper _output;

        protected UnitTestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        protected void Evolve<T>(int generationsCount = 50, EvolutionOptions options = null, Func<EvolutionResult<T>, bool> stopCondition = null) where T : class, IEvolutionaryIndividual, new()
        {
            var geneticEvolution = new GeneticEvolution<T>(options ?? EvolutionOptions.Default);

            geneticEvolution.EvolveUntil(stopCondition ?? (r => r.Generation.Number == generationsCount), result =>
            {
                _output.WriteLine($"Gen. : #{geneticEvolution.CurrentGeneration.Number}");
                _output.WriteLine($"Best : {result.BestIndividual}");
                _output.WriteLine($"Avg  : {result.AverageIndividual}");
                _output.WriteLine($"Worst: {result.WorstIndividual}");
                _output.WriteLine($"------------------------------");
                _output.WriteLine($"Avg.Fitness: {result.AverageFitness}");
                _output.WriteLine("");

                return result.Generation.Number < 500; // safe stop
            });
        }
    }
}
