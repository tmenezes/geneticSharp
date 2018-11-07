using GeneticSharp.Helpers;

namespace GeneticSharp.Breeding
{
    public class RandomCrossoverBreeder<T> where T : new()
    {
        public T Breed(T a, T b)
        {
            var newIndividual = new T();
            var chromosomes = ReflectionHelper.GetProperties<T>();

            foreach (var chromosome in chromosomes)
            {
                var giver = RandomData.GetBool() ? a : b;
                var value = chromosome.GetValue(giver);

                chromosome.SetValue(newIndividual, value);
            }

            return newIndividual;
        }
    }
}
