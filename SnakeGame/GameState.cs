using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameState
    {
        public int Rows { get; }
        public int Columns { get; }
        public GridValue[,] Grid { get; }
        public Directions Dir { get; private set; }
        public int score { get; private set; }
        public bool gameover { get; private set; }

        private readonly LinkedList<Directions> dirChanges = new LinkedList<Directions>();
        private readonly LinkedList<Position> snakepos = new LinkedList<Position>();
        private readonly Random random = new Random();

        public GameState(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Grid = new GridValue[Rows, Columns];
            Dir = Directions.Right;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            int r = Rows / 2;

            for (int i = 0; i <= 3; i++)
            {
                Grid[r, i] = GridValue.Snake;
                snakepos.AddFirst(new Position(r, i));
            }
        }

        private IEnumerable<Position> EmptyPos()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (Grid[i, c] == GridValue.Empty)
                    {
                        yield return new Position(i, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPos());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];

            Grid[pos.Row, pos.Column] = GridValue.Food;
        }

        public Position Headposition()
        {
            return snakepos.First.Value;
        }
        public Position Tailposition()
        {
            return snakepos.Last.Value;
        }

        public IEnumerable<Position> snakepositions()
        {
            return snakepos;
        }
        private void Addhead(Position pos)
        {
            snakepos.AddFirst(pos);
            Grid[pos.Row, pos.Column] = GridValue.Snake;
        }
        private void Removetail()
        {
            Position tail = snakepos.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakepos.RemoveLast();
        }

        private Directions GetLastDirection()
        {
            if(dirChanges.Count == 0)
            {
                return Dir;
            }
            return dirChanges.Last.Value;
        }

        private bool CanChangeDirections(Directions newDir)
        {
            if(dirChanges.Count == 2)
            {
                return false;
            }
            Directions LastDir = GetLastDirection();
            return newDir != LastDir && newDir != LastDir.Opposite();
        }
        public void ChangeDir(Directions dir)
        {
            //checking if change can be made
            if (CanChangeDirections(dir))
            {
                dirChanges.AddFirst(dir);
            }
        }
        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Column < 0 || pos.Column >= Columns;
        }
        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }
            if(newHeadPos == Tailposition()) 
            {
                return GridValue.Empty;
            }
            return Grid[newHeadPos.Row, newHeadPos.Column];
        }

        public void Move()
        {
            if(dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }
            
            Position newHeadpos = Headposition().translate(Dir);
            GridValue hit = WillHit(newHeadpos);

            if(hit == GridValue.Outside || hit == GridValue.Snake)
            {
                gameover = true;
            }
            else if(hit == GridValue.Empty)
            {
                Removetail();
                Addhead(newHeadpos);
            }
            else if(hit == GridValue.Food)
            {
                Addhead(newHeadpos);
                score++;
                AddFood();
            }
        }
    }
}
