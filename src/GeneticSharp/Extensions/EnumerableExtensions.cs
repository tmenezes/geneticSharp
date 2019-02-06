using System;
using System.Collections.Generic;
using AutoBuilder.Helpers;

namespace GeneticSharp.Extensions
{
    public static class EnumerableExtensions
    {
        public static T SelectProportional<T>(this IEnumerable<T> enumerable, Func<T, int> weightFunc)
        {
            int totalWeight = 0;     // this stores sum of weights of all elements before current
            T selected = default(T); // currently selected element
            foreach (var data in enumerable)
            {
                int weight = weightFunc(data); // weight of current element
                int rndValue = RandomData.GetInt(totalWeight + weight); // random value
                if (rndValue >= totalWeight)   // probability of this is weight/(totalWeight+weight)
                    selected = data;           // it is the probability of discarding last selected element and selecting current one instead
                totalWeight += weight;         // increase weight sum
            }
            return selected; // when iterations end, selected is some element of sequence. 
        }
    }
}
