using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Snake
{
    /// <summary>
    /// Directional representation of player
    /// </summary>
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        none
    }

    /// <summary>
    /// Class containing the controller logic for the player
    /// </summary>
    public class SnakePlayer
    {
        private readonly Size _gameSize;
        private readonly FoodManager _foodManager;
        private List<BodyPart> m_SnakeParts = new List<BodyPart>(); // Collection of current snake body parts
        private const int m_CircleRadius = 20; // Determines body part size
        private Direction m_MoveDirection = Direction.none; // Direction of the head
        private int m_PendingSegments = 0; // Number of body parts in queue to be added to the snake

        public int Score { get; private set; }
        public Direction Direction => m_MoveDirection;

        /// <summary>
        /// Object constructor
        /// </summary>
        public SnakePlayer(Size gameSize, FoodManager foodManager)
        {
            // Add 3 body parts to the snake because the snake begins small
            m_SnakeParts.Add(new BodyPart(100, 100, Direction.Right));
            m_SnakeParts.Add(new BodyPart(80, 100, Direction.Right));
            m_SnakeParts.Add(new BodyPart(60, 100, Direction.Right));

            // Need to give an initial direction
            m_MoveDirection = Direction.Right;

            // Currently no body parts queued to be added
            m_PendingSegments = 0;

            // set game size
            _gameSize = gameSize;
            _foodManager = foodManager;
        }

        public void AddScore() => Score++;

        /// <summary>
        /// Adds snake body parts
        /// </summary>
        /// <param name="number">Number of body parts to add</param>
        public void AddBodySegments(int number)
        {
            // Increments m_PendingSegments as it will be processed and the parts added in the next call to MovePlayer()
            m_PendingSegments += number;
        }

        /// <summary>
        /// Moves the snake and processes any pending body segments to be created. Called every frame.
        /// </summary>
        public void MovePlayer()
        {
            // Adds any pending body parts. Note that this processes one body part at a time;
            // if m_PendingSegments > 1, it will require more than one frame to process completely.
            if (m_PendingSegments > 0)
            {
                Point LastPos = m_SnakeParts.Last().GetPosition(); // Adds the body part to the tail
                m_SnakeParts.Add(new BodyPart(LastPos.X, LastPos.Y));
                m_PendingSegments--;
            }

            m_SnakeParts[0].m_Dir = m_MoveDirection; // Set the head direction

            // Moves each snake body part
            for (int i = m_SnakeParts.Count - 1; i >= 0; i--)
            {
                // Translates the body part in accordance with its direction
                switch (m_SnakeParts[i].m_Dir)
                {
                    case Direction.Left:
                        m_SnakeParts[i].AddPosition(new Point(-20, 0));
                        break;
                    case Direction.Right:
                        m_SnakeParts[i].AddPosition(new Point(20, 0));
                        break;
                    case Direction.Down:
                        m_SnakeParts[i].AddPosition(new Point(0, 20));
                        break;
                    case Direction.Up:
                        m_SnakeParts[i].AddPosition(new Point(0, -20));
                        break;
                    default:
                        break;
                }

                // Set the direction of the next part to be the direction of the previous
                // for snake-like movement
                if (i > 0)
                    m_SnakeParts[i].m_Dir = m_SnakeParts[i - 1].m_Dir;
            }
        }

        /// <summary>
        /// Determines whether the snake is intersecting with itself
        /// </summary>
        /// <returns>Whether the snake is intersecting with itself</returns>
        public bool IsSelfIntersecting()
        {
            // Check each snake body part with every other snake body part
            for (int i = 0; i < m_SnakeParts.Count; i++)
            {
                for (int j = 0; j < m_SnakeParts.Count; j++)
                {
                    if (i == j) // Do not want to check a body part with itself
                        continue;
                    BodyPart part1 = m_SnakeParts[i];
                    BodyPart part2 = m_SnakeParts[j];

                    // Collision check logic
                    if ((new Rectangle(part1.GetPosition().X, part1.GetPosition().Y, m_CircleRadius, m_CircleRadius)).IntersectsWith(
                        new Rectangle(part2.GetPosition().X, part2.GetPosition().Y, m_CircleRadius, m_CircleRadius)))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the direction of the snake head
        /// </summary>
        /// <param name="dir">Direction to set the head to</param>
        public void SetDirection(Direction dir)
        {
            // Forbid 180 degree turns
            if (m_MoveDirection == Direction.Left && dir == Direction.Right)
                return;

            if (m_MoveDirection == Direction.Right && dir == Direction.Left)
                return;

            if (m_MoveDirection == Direction.Up && dir == Direction.Down)
                return;

            if (m_MoveDirection == Direction.Down && dir == Direction.Up)
                return;

            // Set the direction if the direction change is legal
            m_MoveDirection = dir;
        }

        /// <summary>
        /// Determines whether any body part is intersecting with the given rectangle
        /// </summary>
        /// <param name="rect">The rectangle to check intsections with</param>
        /// <returns>Whether there was an intersection</returns>
        public bool IsIntersectingRect(Rectangle rect)
        {
            foreach (BodyPart Part in m_SnakeParts) // Check each snake body part
            {
                Point PartPos = Part.GetPosition();

                // Check rectangle intersection with body part
                if (rect.IntersectsWith(new Rectangle(PartPos.X, PartPos.Y, m_CircleRadius, m_CircleRadius)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Called per frame to render the snake body parts
        /// </summary>
        /// <param name="canvas">The graphics object to render on</param>
        public void Draw(Graphics canvas)
        {
            Brush SnakeColor = Brushes.Black;
            List<Rectangle> Rects = GetRects(); // Get the snake body parts, represented as rectangles
            foreach (Rectangle Part in Rects) // Draw each snake body part
            {
                canvas.FillEllipse(SnakeColor, Part); // Draw the snake parts as ellipses
            }
        }

        /// <summary>
        /// Gets the snake body parts, represented as rectangles
        /// </summary>
        /// <returns>A list of snake body parts represented as rectangles</returns>
        public List<Rectangle> GetRects()
        {
            List<Rectangle> Rects = new List<Rectangle>();
            foreach (BodyPart Part in m_SnakeParts) // Return all body parts
            {
                Point PartPos = Part.GetPosition();

                // Every iteration, add a rectangle to the ongoing list representing the body part
                Rects.Add(new Rectangle(PartPos.X, PartPos.Y, m_CircleRadius, m_CircleRadius));
            }
            return Rects;
        }

        public int GetWallDistance(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return SnakeHead.GetPosition().X;

                case Direction.Right:
                    return _gameSize.Width - SnakeHead.GetPosition().X;

                case Direction.Up:
                    return SnakeHead.GetPosition().X;

                case Direction.Down:
                    return _gameSize.Height - SnakeHead.GetPosition().Y;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public int GetFoodDistance(Direction direction)
        {
            int FixNegative(int value) => value < 0 ? Math.Abs(value) * 2 : value;

            switch (direction)
            {
                case Direction.Left:
                    return FixNegative(SnakeHead.GetPosition().X - CurrentFood.GetPosition().X);

                case Direction.Right:
                    return FixNegative(CurrentFood.GetPosition().X - SnakeHead.GetPosition().X);

                case Direction.Up:
                    return FixNegative(SnakeHead.GetPosition().Y - CurrentFood.GetPosition().Y);

                case Direction.Down:
                    return FixNegative(CurrentFood.GetPosition().Y - SnakeHead.GetPosition().Y);

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private BodyPart SnakeHead => m_SnakeParts[0];
        private FoodPellet CurrentFood => _foodManager.LastFood;
    }
}
