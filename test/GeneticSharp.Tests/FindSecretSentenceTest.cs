using System.Linq;
using GeneticSharp.Mutation;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests
{
    public class FindSecretSentenceTest : UnitTestBase
    {
        public FindSecretSentenceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test1()
        {
            var options = new EvolutionOptions
            {
                PopulationSize = 300,
                NaturalSelectionRate = 0.5,
                MutationRate = 0.01,
                Mutation = MutationTypes.Addition,
                CollectionSize = FindSecretSentenceModel.SECRET.Length,
                MinNumberValue = 32, // ascii table start
                MaxNumberValue = 126 // ascii table end
            };

            Evolve<FindSecretSentenceModel>(options: options, stopCondition: r =>
            {
                return r.BestIndividual.Fitness == 100; // stop when achieve 100% fitness
            });
        }
    }

    public class FindSecretSentenceModel : IEvolutionaryIndividual
    {
        public const string SECRET = "Genetic algorithms are awesome! :)";

        public char[] Sentence { get; set; }

        public decimal Fitness { get; private set; }

        public void CalculateFitness()
        {
            var matchedChars = 0;
            for (int i = 0; i < Sentence.Length; i++)
            {
                matchedChars += Sentence[i] == SECRET[i] ? 1 : 0;
            }

            Fitness = matchedChars * 100 / SECRET.Length;
        }

        public override string ToString()
        {
            var sentence = Sentence.Select(c => c.ToString()).Aggregate((one, two) => $"{one}{two}");

            return $"{nameof(Fitness)}: {Fitness}%: {sentence}";
        }
    }
}
