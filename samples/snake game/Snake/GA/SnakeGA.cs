using System;
using System.Collections.Generic;
using System.Linq;
using GeneticSharp;

namespace Snake.GA
{
    public class SnakeGA : IEvolutionaryIndividual
    {
        private static readonly int[] MAX_DISTANCE = { 496, 700, 496, 700 };

        private int _generationId = 0;
        private int _individualId = 0;
        private bool _playingGame = true;
        private int _playedMoves = 0;

        public double[] WallAwareness { get; set; }
        public double[] FoodAwareness { get; set; }
        //public double[] BodyAwareness { get; set; }

        public double AvoidLevel { get; set; }
        public double EagerLevel { get; set; }
        //public double SelfLevel { get; set; }

        // genetic algorithm methods
        public void CalculateFitness()
        {
            _playingGame = true;

            while (_playingGame)
            {
            }
        }

        public decimal Fitness { get; private set; }

        // snake game commands
        public void SnakeGame_OnSnakeMove(SnakePlayer snake)
        {
            _playedMoves++;
            UpdateFitness(snake);

            var directions = Enum.GetValues(typeof(Direction)).OfType<Direction>().Where(d => d != Direction.none).OrderBy(d => (int)d).ToList();

            //var avoidRates = directions.Select((d, i) => (dir: d, rate: WallAwareness[i] / 100 * (MAX_DISTANCE[(int)d] - snake.GetWallDistance(d)) * AvoidLevel / 100));
            //var eagerRates = directions.Select((d, i) => (dir: d, rate: FoodAwareness[i] / 100 * (MAX_DISTANCE[(int)d] - snake.GetFoodDistance(d)) * EagerLevel / 100));
            var eagerRates = directions.Select((d, i) => (dir: d, rate: FoodAwareness[i] / 100 * (100d * snake.GetFoodDistance(d) / MAX_DISTANCE[(int)d]) * EagerLevel / 100));

            // implement random factor to adjust rates randomly
            //var randomFactor = new []
            //{
            //    RandomData.GetInt(0, Convert.ToInt32(AvoidLevel * 100)),
            //    RandomData.GetInt(0, Convert.ToInt32(EagerLevel * 100)),
            //};

            //var avoidRateAvg = avoidRates.Average(e => e.rate); // MIN values are the best to take...
            var eagerRateAvg = eagerRates.Average(e => e.rate); // MAX values are the best to take...

            //var result = eagerRateAvg > avoidRateAvg
            //    ? eagerRates.OrderByDescending(e => e.rate)
            //    : avoidRates.OrderBy(e => e.rate);
            var result = eagerRates.Where(e => e.rate > 0).OrderBy(e => e.rate);

            //var notAllowedDirection = GetOppositeDirection(snake.Direction);
            //var maxRate = result.First(e => e.dir != notAllowedDirection);
            var maxRate = result.Any() ? result.First() : eagerRates.OrderBy(e => e.rate).First();

            snake.SetDirection(maxRate.dir);
            ShowDebugData(maxRate.dir, snake, eagerRates.Select(i => i.rate), eagerRates.Select(i => i.rate));
        }

        public void SnakeGame_OnGameOver(SnakePlayer snake)
        {
            UpdateFitness(snake);
            _playingGame = false;
        }

        // others
        public void SetGeneration(Generation<SnakeGA> generation)
        {
            _generationId = generation.Number;
            _individualId = generation.Population.ToList().FindIndex(i => i == this) + 1;
        }

        // helpers
        private void UpdateFitness(SnakePlayer snake)
        {
            Fitness = snake.Score * 2 + _playedMoves / 100m;
        }

        private void ShowDebugData(Direction direction, SnakePlayer snake, IEnumerable<double> avoidRates, IEnumerable<double> eagerRates)
        {
            var directions = Enum.GetValues(typeof(Direction)).OfType<Direction>().Where(d => d != Direction.none).ToList();
            var wallAwareness = WallAwareness.Select(v => v.ToString("F1")).Aggregate((a, b) => $"{a}, {b}");
            var wallDistances = directions.Select(d => snake.GetWallDistance(d).ToString("F1")).Aggregate((a, b) => $"{a}, {b}");
            var avoidRatesDbg = directions.Select(d => avoidRates.ElementAt((int)d).ToString("F1")).Aggregate((a, b) => $"{a}, {b}");
            var foodAwareness = FoodAwareness.Select(v => v.ToString("F1")).Aggregate((a, b) => $"{a}, {b}");
            var foodDistances = directions.Select(d => snake.GetFoodDistance(d).ToString("F1")).Aggregate((a, b) => $"{a}, {b}");
            var eagerRatesDbg = directions.Select(d => eagerRates.ElementAt((int)d).ToString("F1")).Aggregate((a, b) => $"{a}, {b}");

            var debug = string.Concat(
                $"Gen. #{_generationId:00} / Snake #{_individualId:00} - {Fitness:F} pts", Environment.NewLine, Environment.NewLine,
                "Wall Aware: ", wallAwareness, Environment.NewLine,
                "Food Aware: ", foodAwareness, Environment.NewLine,
                "Avoid. Lvl: ", AvoidLevel.ToString("F"), Environment.NewLine,
                "Eager  Lvl: ", EagerLevel.ToString("F"), Environment.NewLine, Environment.NewLine,
                "Wall Dist.: ", wallDistances, Environment.NewLine,
                "Avoid Rate: ", avoidRatesDbg, Environment.NewLine,
                "Food Dist.: ", foodDistances, Environment.NewLine,
                "Eager Rate: ", eagerRatesDbg, Environment.NewLine, Environment.NewLine,
                "Dir. Taken: ", direction
            );

            Program.NotifyDebugData(debug);
        }

        private static Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Right: return Direction.Left;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.none: return Direction.none;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
