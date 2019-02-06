using System;
using System.Linq;
using System.Windows.Forms;
using Snake.GA;

namespace Snake
{
    static class Program
    {
        public static Evolution Evolution = new Evolution();
        private static SnakeForm _gameForm;

        /// <summary>
        /// The main entry point for the application.
        /// Initializes the application and opens the Snake game window.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // !!!!! Manual !!!!!
            //Application.Run(new SnakeForm());

            // !!!!! Automated !!!!!
            _gameForm = SnakeForm.ForAutomatedControl(1); // 2x

            Evolution.GeneticEvolution.BeforeNaturalSelection += (individual, generation) =>
            {
                //individual.AvoidLevel = 1;
                //individual.EagerLevel = 100;
                //individual.WallAwareness = new[] { 100d, 100, 100, 100 };
                //individual.FoodAwareness = new[] { 100d, 100, 100, 100 };

                individual.SetGeneration(generation);
                _gameForm.OnSnakeMove += individual.SnakeGame_OnSnakeMove;
                _gameForm.OnGameOver += individual.SnakeGame_OnGameOver;
                _gameForm.StartNewGame();
            };

            Evolution.GeneticEvolution.AfterNaturalSelection += (individual, generation) =>
            {
                _gameForm.OnSnakeMove -= individual.SnakeGame_OnSnakeMove;
                _gameForm.OnGameOver -= individual.SnakeGame_OnGameOver;
            };

            Evolution.GeneticEvolution.GenerationEvolved += result =>
            {
                _gameForm.Invoke((Action)(() => _gameForm.SetGenerationResult(result)));
            };

            Evolution.Start();
            Application.Run(_gameForm);
        }

        internal static void NotifyDebugData(string data)
        {
            _gameForm.Invoke((Action)(() => _gameForm.SetDebugData(data)));

        }

        private static void GeneticEvolution_GenerationEvolved(GeneticSharp.EvolutionResult<SnakeGA> generationResult)
        {
            throw new NotImplementedException();
        }

        private static readonly Random _random = new Random(DateTime.Now.GetHashCode());
        private static Direction GetDirection()
        {
            return Enum.GetValues(typeof(Direction)).OfType<Direction>().ElementAt(_random.Next(0, 4));
        }
    }
}
