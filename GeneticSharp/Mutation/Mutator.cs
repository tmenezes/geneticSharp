using System;
using System.Collections.Generic;
using GeneticSharp.Extensions;
using GeneticSharp.Helpers;

namespace GeneticSharp.Mutation
{
    public class Mutator
    {
        public static Population<T> Mutate<T>(Population<T> population, EvolutionOptions options) where T : class, IEvolutionaryIndividual, new()
        {
            var mutator = options.GetMutation<T>();

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
    }
}