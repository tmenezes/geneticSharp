using System;

namespace GeneticSharp.Mutation
{
    public class Mutator
    {
        public static Population<T> Mutate<T>(Population<T> population, EvolutionOptions options) where T : class, IEvolutionaryIndividual, new()
        {
            var mutator = new UniformMutation<T>(options);

            foreach (var individual in population)
            {
                mutator.Mutate(individual);
            }

            return population;
        }

        internal static void SetOrFallback(Action setAction, Action fallbackAction)
        {
            try
            {
                setAction();
            }
            catch (Exception)
            {
                fallbackAction();
            }
        }

        internal static bool TryGetValue(object input, out decimal value)
        {
            try
            {
                value = Convert.ToDecimal(input);
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
    }
}