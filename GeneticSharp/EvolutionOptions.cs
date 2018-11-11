using GeneticSharp.Selection;

namespace GeneticSharp
{
    public class EvolutionOptions
    {
        public int PopulationSize { get; set; } = 100;

        public double NaturalSelectionRate { get; set; } = 0.75; // set percentage of the population that will not "die"
        public NaturalSelectionTypes NaturalSelectionType { get; set; } = NaturalSelectionTypes.EliteSelection;

        public double MutationRate { get; set; } = 0.01;

        public int CollectionSize { get; set; } = 10;
        public int MinNumberValue { get; set; } = 0;
        public int MaxNumberValue { get; set; } = int.MaxValue;

        public static readonly EvolutionOptions Default = new EvolutionOptions();
    }
}