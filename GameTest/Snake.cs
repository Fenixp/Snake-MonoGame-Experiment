using CommonLib.Common;
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
            _squares.Add(new Square(0, 0, ContentReader.Tilesets.SnakeSpriteSheet.BlackSquare));
        }

        public void Eat(Square square)
        {
            _squares.Add(square);
            _squares.AddRange(new List<Square> 
            { 
                Fatty(square),
                Fatty(square),
                Fatty(square)
            });
        }

        public Square Fatty(Square square)
        {
            return new Square(square.XCoord, square.YCoord, ContentReader.Tilesets.SnakeSpriteSheet.BlackSquare);
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
                RedrawTiles();
            }
        }

        private void RedrawTiles()
        {
            for (int i = 0; i < _squares.Count; i++)
            {
                if (i == _squares.Count - 1)
                {
                    _squares[i].TileInfo = GetDirectionalTileTail(_squares[i].Direction);
                }
                else if (i == 0)
                {
                    _squares[i].TileInfo = GetDirectionalTileHead(_squares[i].Direction);
                }
                else
                {
                    _squares[i].TileInfo = GetDirectionalTile(_squares[i].Direction, i);
                }
            }
        }



        private void MoveInternal(DirectionEnum direction)
        {
            for (int i = _squares.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    MoveInDirection(_squares[i], direction);
                    _squares[i].Direction = direction;
                }
                else
                {
                    _squares[i].XCoord = _squares[i - 1].XCoord;
                    _squares[i].YCoord = _squares[i - 1].YCoord;
                    _squares[i].Direction = _squares[i - 1].Direction;
                }
            }
        }

        private TileInfo GetDirectionalTileHead(DirectionEnum direction)
        {
            switch (direction)
            { 
                case DirectionEnum.up:
                    return ContentReader.Tilesets.SnakeSpriteSheet.HeadBlockTop;
                case DirectionEnum.down:
                    return ContentReader.Tilesets.SnakeSpriteSheet.HeadBlockBottom;
                case DirectionEnum.left:
                    return ContentReader.Tilesets.SnakeSpriteSheet.HeadBlockLeft;
                case DirectionEnum.right:
                    return ContentReader.Tilesets.SnakeSpriteSheet.HeadBlockRight;
            }
            return ContentReader.Tilesets.SnakeSpriteSheet.HeadBlockTop;
        }

        private TileInfo GetDirectionalTileTail(DirectionEnum direction)
        {
            switch (direction)
            { 
                case DirectionEnum.up:
                    return ContentReader.Tilesets.SnakeSpriteSheet.TailBlockBottom;
                case DirectionEnum.down:
                    return ContentReader.Tilesets.SnakeSpriteSheet.TailBlockTop;
                case DirectionEnum.left:
                    return ContentReader.Tilesets.SnakeSpriteSheet.TailBlockLeft;
                case DirectionEnum.right:
                    return ContentReader.Tilesets.SnakeSpriteSheet.TailBlockRight;
            }
            return ContentReader.Tilesets.SnakeSpriteSheet.TailBlockTop;
        }

        private TileInfo GetDirectionalTile(DirectionEnum direction, int squarePosition)
        {
            if (_squares[squarePosition - 1].Direction == _squares[squarePosition].Direction)
            {
                switch (direction)
                {
                    case DirectionEnum.up:
                        return ContentReader.Tilesets.SnakeSpriteSheet.SnakeBlockTop;
                    case DirectionEnum.down:
                        return ContentReader.Tilesets.SnakeSpriteSheet.SnakeBlockTop;
                    case DirectionEnum.left:
                        return ContentReader.Tilesets.SnakeSpriteSheet.SnakeBlockLeft;
                    case DirectionEnum.right:
                        return ContentReader.Tilesets.SnakeSpriteSheet.SnakeBlockRight;
                }
            }
            else
            {
                return GetCorners(_squares[squarePosition - 1], _squares[squarePosition + 1], _squares[squarePosition]);
            }
            return ContentReader.Tilesets.SnakeSpriteSheet.SnakeBlockTop;
        }

        private TileInfo GetCorners(Square square1, Square square2, Square corner)
        {
            Square yLineSquare;
            Square xLineSquare;

            if (square1.YCoord == corner.YCoord)
            {
                yLineSquare = square1;
                xLineSquare = square2;
            }
            else
            {
                yLineSquare = square2;
                xLineSquare = square1;
            }

            if (yLineSquare.XCoord > xLineSquare.XCoord && yLineSquare.YCoord > xLineSquare.YCoord)
            {
                return ContentReader.Tilesets.SnakeSpriteSheet.TransitionRightTop;
            }
            else if (yLineSquare.XCoord > xLineSquare.XCoord && yLineSquare.YCoord < xLineSquare.YCoord)
            {
                return ContentReader.Tilesets.SnakeSpriteSheet.TransitionRightBottom;
            }
            else if (yLineSquare.XCoord < xLineSquare.XCoord && yLineSquare.YCoord > xLineSquare.YCoord)
            {
                return ContentReader.Tilesets.SnakeSpriteSheet.TransitionLeftTop;
            }
            else
            {
                return ContentReader.Tilesets.SnakeSpriteSheet.TransitionLeftBottom;
            }
        }

        private void MoveInDirection(Square square, DirectionEnum direction)
        {
            square.Direction = direction;
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
