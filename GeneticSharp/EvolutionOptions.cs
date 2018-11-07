namespace GeneticSharp
{
    public class EvolutionOptions
    {
        public int GenerationQuantity { get; set; } = 100;
        public double NaturalSelectionRate { get; set; } = 0.75; // set percetange of the population that will not "die"
        public double MutationRate { get; set; } = 0.05;

        public static EvolutionOptions Default = new EvolutionOptions();
    }
}