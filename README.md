# GeneticSharp
GeneticSharp is a **.Net Core** library that  that handles the mecanics of a generic algorithm implementation. It automatically creates the ``population``, do by itself the ``reproduction`` and ``mutation`` phases according with the strategy and configuration set.

GeneticSharp do the hard job for you and let you focus on the most important piece, the problem you are solving. You can direct focus on designing your model, chromosses, and the fitness method.

GeneticSharp accepts as a model any C# POCO class. It needs to be a class, needs to implement the ``IEvolutionaryIndividual`` interface and to have the default constructor.

## Simplest Usage
```csharp
using GeneticSharp;
// ...
var geneticEvolution = new GeneticEvolution<MyModel>();

// gen1
var gen1Result = geneticEvolution.Evolve();
Console.WriteLine(gen1Result.BestIndividual);

// gen2
var gen2Result = geneticEvolution.Evolve();
Console.WriteLine(gen2Result.BestIndividual);
```

## Configuration
Supported variables
1. Population Size: amount of individuals (population) per generation
1. Natural Selection Rate: population percetage that is selected to reproduce and generate new individuals to the next gen
1. Mutation Rate
1. Collection Types Sizes

## Types supporteds
1. string
1. int, short, long
1. float, double, decimal
1. Nullable
1. bool 
1. DateTime
1. IEnumerable<T>, IList<T>, List<T>, Array
1. Enums