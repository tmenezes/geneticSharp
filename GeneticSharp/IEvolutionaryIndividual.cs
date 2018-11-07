namespace GeneticSharp
{
    public interface IEvolutionaryIndividual
    {
        decimal Fitness { get; }
        void CalculateFitness();
    }
}