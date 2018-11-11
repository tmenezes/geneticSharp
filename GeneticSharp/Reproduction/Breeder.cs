﻿using System.Collections.Generic;
using System.Linq;
using AutoBuilder.Helpers;

namespace GeneticSharp.Reproduction
{
    public class Breeder
    {
        public static Population<T> Breed<T>(Population<T> population, EvolutionOptions options) where T : class, IEvolutionaryIndividual, new()
        {
            var newIndividuals = new List<T>();
            var breeder = new UniformCrossover<T>(options);

            var orderedPopulation = population.OrderByDescending(i => i.Fitness).ToList();

            while (newIndividuals.Count != options.PopulationSize)
            {
                foreach (var individual in orderedPopulation)
                {
                    var partner = population.ElementAt(RandomData.GetInt(population.Count()));
                    var newIndividual = breeder.Produce(individual, partner);

                    newIndividuals.Add(newIndividual);
                    if (newIndividuals.Count == options.PopulationSize)
                        break;
                }
            }
            return new Population<T>(newIndividuals);
        }
    }
}
