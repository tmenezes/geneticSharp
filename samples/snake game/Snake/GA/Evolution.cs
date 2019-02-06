using System.Threading.Tasks;
using GeneticSharp;
using GeneticSharp.Mutation;
using GeneticSharp.Reproduction;

namespace Snake.GA
{
    public class Evolution
    {
        private readonly int _maxGenerations;
        private readonly GeneticEvolution<SnakeGA> _geneticEvolution;

        public GeneticEvolution<SnakeGA> GeneticEvolution => _geneticEvolution;

        public Evolution(int maxGenerations = 100)
        {
            _maxGenerations = maxGenerations;

            var options = new EvolutionOptions
            {
                PopulationSize = 100,
                MaxNumberValue = 100,
                CollectionSize = 4,
                Crossover = CrossoverTypes.Slice,
                Mutation = MutationTypes.Addition,
            };
            _geneticEvolution = new GeneticEvolution<SnakeGA>(options);
        }

        public void Start(int delayMs = 1000)
        {
            Task.Delay(delayMs).ContinueWith(t => _geneticEvolution.EvolveUntil(g => g.Generation.Number == _maxGenerations));
        }
    }
}
