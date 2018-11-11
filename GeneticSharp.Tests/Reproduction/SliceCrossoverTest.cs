using System.Linq;
using FluentAssertions;
using GeneticSharp.Reproduction;
using Xunit;
using Xunit.Abstractions;

namespace GeneticSharp.Tests.Reproduction
{
    public class SliceCrossoverTest
    {
        private readonly ITestOutputHelper _output;

        public SliceCrossoverTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Should_slice_and_Produce_Child_properly()
        {
            for (int x = 0; x < 100; x++)
            {
                var parentA = new CrossoverTestModel(new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                var parentB = new CrossoverTestModel(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });

                var options = new EvolutionOptions() { CollectionSize = parentA.Bits.Count }; // 10 items
                var child = new SliceCrossover<CrossoverTestModel>(options).Produce(parentA, parentB);
                _output.WriteLine(child.ToString());

                // asserts
                var cutPoint1 = child.Bits.Select((b, i) => (bit: b, index: i)).First(e => e.bit == 1).index;
                var cutPoint2 = child.Bits.Select((b, i) => (bit: b, index: i)).First(e => e.bit == 0 && e.index > cutPoint1).index;
                var bitsBefore1stCutPoint = child.Bits.Where((b, i) => i < cutPoint1).ToList();
                var bitsBetweenCutPoints = child.Bits.Where((b, i) => i >= cutPoint1 && i < cutPoint2).ToList();
                var bitsAfter2ndCutPoint = child.Bits.Where((b, i) => i >= cutPoint2).ToList();

                child.Bits.Should().HaveSameCount(parentA.Bits);
                child.Bits.Should().HaveCount(bitsBefore1stCutPoint.Count + bitsBetweenCutPoints.Count + bitsAfter2ndCutPoint.Count);
                bitsBefore1stCutPoint.Should().AllBeEquivalentTo(0);
                bitsBetweenCutPoints.Should().AllBeEquivalentTo(1);
                bitsAfter2ndCutPoint.Should().AllBeEquivalentTo(0);
            }
        }
    }
}