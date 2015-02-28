using CommonLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest
{
    public class Square
    {
        public Square(int xCoord, int yCoord, TileInfo tileInfo)
        {
            XCoord = xCoord;
            YCoord = yCoord;
            TileInfo = tileInfo;
        }

        public TileInfo TileInfo;
        public int XCoord;
        public int YCoord;
        public DirectionEnum Direction;
    }
}
