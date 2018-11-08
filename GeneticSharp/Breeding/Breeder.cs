using System.Collections.Generic;
using System.Linq;
using GeneticSharp.Helpers;

namespace GeneticSharp.Breeding
{
    public class Breeder
    {
        public static Population<T> Breed<T>(Population<T> population, EvolutionOptions options) where T : class, IEvolutionaryIndividual, new()
        {
            var newIndividuals = new List<T>();
            var breeder = new RandomCrossoverBreeder<T>(options);

            var orderedPopulation = population.OrderByDescending(i => i.Fitness).ToList();

            while (newIndividuals.Count != options.GenerationQuantity)
            {
                foreach (var individual in orderedPopulation)
                {
                    var partner = population.ElementAt(RandomData.GetInt(population.Count()));
                    var newIndividual = breeder.Breed(individual, partner);

                    newIndividuals.Add(newIndividual);
                    if (newIndividuals.Count == options.GenerationQuantity)
                        break;
                }
            }
            return new Population<T>(newIndividuals);
        }
    }
}
