using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLib.Common
{
    public class TileInfo
    {
        Texture2D _texture;
        Rectangle _rectangle;
        string _name;

        public TileInfo(string name, Rectangle size, Texture2D texture)
        {
            _texture = texture;
            _name = name;
            _rectangle = size;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Texture2D Texture
        {
            get 
            {
                return _texture;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return _rectangle;
            }
        }
    }
}
