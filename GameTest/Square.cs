using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest
{
    public class Square
    {
        private int _xCoord;
        private int _yCoord;

        public Square(int xCoord, int yCoord)
        {
            _xCoord = xCoord;
            _yCoord = yCoord;
        }

        public int XCoord
        {
            get
            {
                return _xCoord;
            }
            set
            {
                _xCoord = value;
            }
        }

        public int YCoord
        {
            get
            {
                return _yCoord;
            }
            set
            {
                _yCoord = value;
            }
        }
    }
}
