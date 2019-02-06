using System;
using System.Drawing;
using System.Windows.Forms;
using GeneticSharp;
using Snake.GA;

namespace Snake
{
    public delegate void SnakeEventHandler(SnakePlayer snake);

    public partial class SnakeForm : Form, IMessageFilter
    {
        SnakePlayer Player1;
        FoodManager FoodMngr;
        Random r = new Random();
        private int score = 0;
        private bool _isAutomatedControlled;

        public event SnakeEventHandler OnSnakeMove;
        public event SnakeEventHandler OnGameOver;

        public SnakeForm()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            this.FormClosed += (s, e) => Application.RemoveMessageFilter(this);

            ResetGame();

            ScoreTxtBox.Text = score.ToString();
            speedSlider.Value = GameTimer.Interval;
        }

        public static SnakeForm ForAutomatedControl(int speedUpRate = 1)
        {
            var form = new SnakeForm { _isAutomatedControlled = true };
            form.GameTimer.Interval /= speedUpRate;
            return form;
        }

        // game commands
        public void StartNewGame()
        {
            void startNewGame()
            {
                ResetGame();
                GameTimer.Enabled = true;
            }

            if (InvokeRequired)
            {
                Invoke((Action)startNewGame);
            }
            else
            {
                startNewGame();
            }
        }

        public void ResetGame()
        {
            FoodMngr = new FoodManager(GameCanvas.Width, GameCanvas.Height);
            FoodMngr.AddRandomFood();
            Player1 = new SnakePlayer(GameCanvas.Size, FoodMngr);
            score = 0;
            Input.Clear();
        }

        public void SetGameOver(string reason)
        {
            GameTimer.Enabled = false;

            if (!_isAutomatedControlled)
            {
                MessageBox.Show($"{reason} - GAME OVER"); // Display game-over message
            }

            if (_isAutomatedControlled)
            {
                OnGameOver?.Invoke(Player1);
            }
            else
            {
                ResetGame();
            }
        }

        public void ToggleTimer()
        {
            GameTimer.Enabled = !GameTimer.Enabled;
        }

        private void DoGameLoop()
        {
            if (_isAutomatedControlled)
            {
                OnSnakeMove?.Invoke(Player1);
            }
            else
            {
                SetPlayerDirection();
            }

            Player1.MovePlayer();
            CheckForCollisions();
            GameCanvas.Invalidate();
        }

        private void CheckForCollisions()
        {
            if (Player1.IsSelfIntersecting()) // Check for collisions with itself
                SetGameOver("Hit itself");    // If so, trigger the game-over screen

            if (Player1.IsIntersectingRect(new Rectangle(-100, 0, 100, GameCanvas.Height)))
                SetGameOver("Hit left wall");

            if (Player1.IsIntersectingRect(new Rectangle(0, -100, GameCanvas.Width, 100)))
                SetGameOver("Hit top wall");

            if (Player1.IsIntersectingRect(new Rectangle(GameCanvas.Width, 0, 100, GameCanvas.Height)))
                SetGameOver("Hit right wall");

            if (Player1.IsIntersectingRect(new Rectangle(0, GameCanvas.Height, GameCanvas.Width, 100)))
                SetGameOver("Hit bottom wall");

            // Is hitting food
            var snakeRects = Player1.GetRects();
            foreach (var rect in snakeRects)
            {
                if (FoodMngr.IsIntersectingRect(rect, true))
                {
                    FoodMngr.AddRandomFood();
                    Player1.AddBodySegments(1);
                    Player1.AddScore();
                    score++;
                    ScoreTxtBox.Text = score.ToString();
                }
            }
        }

        private void SetPlayerDirection()
        {
            if (Input.IsKeyDown(Keys.Left))
            {
                Player1.SetDirection(Direction.Left);
            }
            else if (Input.IsKeyDown(Keys.Right))
            {
                Player1.SetDirection(Direction.Right);
            }
            else if (Input.IsKeyDown(Keys.Up))
            {
                Player1.SetDirection(Direction.Up);
            }
            else if (Input.IsKeyDown(Keys.Down))
            {
                Player1.SetDirection(Direction.Down);
            }
        }
        
        // debug methods
        public void SetDebugData(string data)
        {
            lblMessage.Text = data;
        }

        public void SetGenerationResult(EvolutionResult<SnakeGA> result)
        {
            var detail = string.Concat(
                $"Gen. #{result.Generation.Number:00}: ",
                $"Best={result.BestIndividual.Fitness:F}", " / ",
                $"Avg.={result.AverageIndividual.Fitness:F}", " / ",
                $"Worst={result.WorstIndividual.Fitness:F}"
            );
            lstGenResult.Items.Insert(0, detail);
        }

        // form events
        public bool PreFilterMessage(ref Message msg)
        {
            if (msg.Msg == 0x0101) //KeyUp
                Input.SetKey((Keys)msg.WParam, false);
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 0x100) //KeyDown
                Input.SetKey((Keys)msg.WParam, true);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Player1.Draw(canvas);
            FoodMngr.Draw(canvas);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            DoGameLoop();
        }

        private void Start_Btn_Click(object sender, EventArgs e)
        {
            ToggleTimer();
        }

        private void SpeedSlider_Scroll(object sender, EventArgs e)
        {
            GameTimer.Interval = speedSlider.Value;
        }        
    }
}
