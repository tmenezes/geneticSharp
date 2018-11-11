using System.Linq;
using FluentAssertions;
using GeneticSharp.Reproduction;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests.Reproduction
{
    public class SinglePointCrossoverTest
    {
        private readonly ITestOutputHelper _output;

        public SinglePointCrossoverTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Should_cut_and_Produce_Child_properly()
        {
            for (int x = 0; x < 100; x++)
            {
                var parentA = new CrossoverTestModel(new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                var parentB = new CrossoverTestModel(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });

                var options = new EvolutionOptions() { CollectionSize = parentA.Bits.Count }; // 10 items
                var child = new SinglePointCrossover<CrossoverTestModel>(options).Produce(parentA, parentB);
                _output.WriteLine(child.ToString());

                // asserts
                var cutPoint = child.Bits.Select((b, i) => (bit: b, index: i)).First(e => e.bit == 1).index;
                var bitsBeforeCutPoint = child.Bits.Where((b, i) => i < cutPoint).ToList();
                var bitsAfterCutPoint = child.Bits.Where((b, i) => i >= cutPoint).ToList();

                child.Bits.Should().HaveSameCount(parentA.Bits);
                child.Bits.Should().HaveCount(bitsBeforeCutPoint.Count + bitsAfterCutPoint.Count);
                bitsBeforeCutPoint.Should().AllBeEquivalentTo(0);
                bitsAfterCutPoint.Should().AllBeEquivalentTo(1);
            }
        }
    }
}
