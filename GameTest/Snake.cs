using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest
{
    public class Snake
    {
        private int _xLimit;
        private int _yLimit;

        List<Square> _squares;

        public IEnumerable<Square> Squares
        {
            get
            {
                return _squares;
            }
        }

        public Square Head
        {
            get
            {
                return _squares[0];
            }
        }

        public DirectionEnum Direction = DirectionEnum.none;
        public Neighbours Neighbours = new Neighbours();
        public bool Alive = true;

        public Snake(int xLimit, int yLimit)
        {
            _xLimit = xLimit;
            _yLimit = yLimit;
            _squares = new List<Square>();
            _squares.Add(new Square(0, 0));
        }

        public void Eat(Square square)
        {
            _squares.Add(square);
            _squares.AddRange(new List<Square> 
            { 
                fatty(square),
                fatty(square),
                fatty(square)
            });
        }

        public Square fatty(Square square)
        {
            return new Square(square.XCoord, square.YCoord);
        }

        public void Move()
        {
            if (Direction == DirectionEnum.none)
            {
                return;
            }

            object obstacle = Neighbours.GetDirection(Direction);

            ProcessObstacle(obstacle);
            
            if (Alive)
            {
                MoveInternal(Direction);
            }
        }

        private void MoveInternal(DirectionEnum direction)
        {
            for (int i = _squares.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    MoveInDirection(_squares[i], direction);
                }
                else
                {
                    _squares[i].XCoord = _squares[i - 1].XCoord;
                    _squares[i].YCoord = _squares[i - 1].YCoord;
                }
            }
        }

        private void MoveInDirection(Square square, DirectionEnum direction)
        {
            switch (direction)
            { 
                case DirectionEnum.up:
                    square.YCoord--;
                    if (square.YCoord < 0)
                    {
                        square.YCoord = _yLimit - 1;
                    }
                    break;
                case DirectionEnum.down:
                    square.YCoord++;
                    if (square.YCoord > _yLimit)
                    {
                        square.YCoord = 0;
                    }
                    break;
                case DirectionEnum.left:
                    square.XCoord--;
                    if (square.XCoord < 0)
                    {
                        square.XCoord = _xLimit - 1;
                    }
                    break;
                case DirectionEnum.right:
                    square.XCoord++;
                    if (square.XCoord > _xLimit)
                    {
                        square.XCoord = 0;
                    }
                    break;
            }
        }

        private void ProcessObstacle(object obstacle)
        {
            if (obstacle == null)
            {
                return;
            }

            Type obstacleType = obstacle.GetType();
            if (obstacleType == typeof(Square) && _squares.Contains((Square)obstacle))
            {
                Alive = false;
            }
            else if(obstacleType == typeof(Square))
            {
                Eat((Square)obstacle);
            }
        }
    }
}
