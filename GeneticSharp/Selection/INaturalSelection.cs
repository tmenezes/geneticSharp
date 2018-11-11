namespace GeneticSharp.Selection
{
    public interface INaturalSelection<T> where T : class, IEvolutionaryIndividual, new()
    {
        Population<T> Select(Population<T> population);
    }

    public enum SelectionTypes
    {
        /// <summary>
        /// Selects the best individuals, no repetition is allowed.
        /// </summary>
        Truncate,

        /// <summary>
        /// Selects individuals based on its Fitness. Can reduce variability by because it allows individuals to be selected more than one time.
        /// </summary>
        Proportional
    }
}