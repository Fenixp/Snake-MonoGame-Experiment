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
            //Todo: Rewrite the direction alghorytms. They're embarassing.
            switch (direction)
            { 
                case DirectionEnum.up:
                    MoveCoordAdd(0, -1, square);
                    break;
                case DirectionEnum.down:
                    MoveCoordAdd(0, 1, square);
                    break;
                case DirectionEnum.left:
                    MoveCoordAdd(-1, 0, square);
                    break;
                case DirectionEnum.right:
                    MoveCoordAdd(1, 0, square);
                    break;
            }
        }

        private void MoveCoordAdd(int x, int y, Square square)
        {
            square.XCoord = CalculateCoords(square.XCoord + x, _xLimit);
            square.YCoord = CalculateCoords(square.YCoord + y, _yLimit);
        }

        private int CalculateCoords(int coord, int limit)
        {
            if (coord < 0)
            {
                coord = coord + limit;
            }
            return coord % (limit);
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
