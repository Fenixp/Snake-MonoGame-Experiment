using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest
{
    public class Neighbours
    {
        private Dictionary<DirectionEnum, object> _directions = new Dictionary<DirectionEnum, object>() 
        { 
            { DirectionEnum.up, null }, 
            { DirectionEnum.down, null }, 
            { DirectionEnum.left, null }, 
            { DirectionEnum.right, null } 
        };

        public void SetDirection(DirectionEnum direction, object obj)
        {
            _directions[direction] = obj;
        }

        public object GetDirection(DirectionEnum direction)
        {
            return _directions[direction];
        }
    }
}
