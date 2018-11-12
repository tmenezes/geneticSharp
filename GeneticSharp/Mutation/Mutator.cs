namespace GeneticSharp.Mutation
{
    public class Mutator
    {
        public static Population<T> Mutate<T>(Population<T> population, EvolutionOptions options) where T : class, IEvolutionaryIndividual, new()
        {
            var mutator = new UniformMutator<T>(options);

            foreach (var individual in population)
            {
                mutator.Mutate(individual);
            }

            return population;
        }
    }
}