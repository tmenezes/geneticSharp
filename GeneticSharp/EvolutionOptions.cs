namespace GeneticSharp
{
    public class EvolutionOptions
    {
        public int GenerationQuantity { get; set; } = 100;
        public double NaturalSelectionRate { get; set; } = 0.75; // set percentage of the population that will not "die"
        public double MutationRate { get; set; } = 0.05;
        public int CollectionTypesSizes { get; set; } = 5;

        public static readonly EvolutionOptions Default = new EvolutionOptions();
    }
}