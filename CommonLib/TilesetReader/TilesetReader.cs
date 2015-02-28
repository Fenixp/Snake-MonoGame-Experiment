using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonLib.Common
{
    public static class TilesetReader
    {
        //public static string ListsFolder = String.Empty;

        private static Dictionary<string, Dictionary<string, TileInfo>> _tilesets = new Dictionary<string, Dictionary<string, TileInfo>>();

        public static TileInfo GetSprite(string tileset, string spriteName)
        {
            TileInfo info = TryGetSprite(tileset, spriteName);
            if (info == null)
            {
                throw new Exception("SpriteName unknown");
            }
            else
            {
                return info;
            }
        }

        public static TileInfo TryGetSprite(string tileset, string spriteName)
        {
            if (_tilesets.ContainsKey(tileset))
            {
                if (_tilesets[tileset].ContainsKey(spriteName))
                {
                    return _tilesets[tileset][spriteName];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                ReadTileset(tileset);
                return TryGetSprite(tileset, spriteName);
            }
        }

        public static Dictionary<string, TileInfo> GetSprites(string tileset)
        {
            if (_tilesets.ContainsKey(tileset))
            {
                return _tilesets[tileset];
            }
            else
            {
                ReadTileset(tileset);
                return GetSprites(tileset);
            }
        }

        public static List<TileInfo> GetAnimation(string tileset, string animationName)
        {
            int i = 1;
            TileInfo info = TryGetSprite(tileset, animationName + (i++).ToString());
            List<TileInfo> infos = new List<TileInfo>();

            while(info != null)
            {
                infos.Add(info);
                info = TryGetSprite(tileset, animationName + (i++).ToString());
            }

            if (infos.Count == 0)
            {
                throw new Exception("AnimationName unknown"); 
            }

            return infos;
        }

        private static void ReadTileset(string tileset)
        {
            List<string> Lines = ReadLines(tileset);
            string FileName = Lines[0];

            if (FileName == null)
            {
                throw new Exception("Incorrect tileset file format");
            }

            Lines.RemoveAt(0);
            _tilesets.Add(tileset, ProcessLines(Lines, ContentSettings.Content.Load<Texture2D>(FileName)));
        }

        private static Dictionary<string, TileInfo> ProcessLines(IEnumerable<string> Lines, Texture2D tileSet)
        {
            Tuple<Rectangle, string> SpriteInfo;
            Dictionary<string, TileInfo> Tiles = new Dictionary<string,TileInfo>();

            foreach (string str in Lines)
            {
                SpriteInfo = TrySplitSprite(str);

                Tiles.Add(SpriteInfo.Item2, new TileInfo(SpriteInfo.Item2, SpriteInfo.Item1, tileSet));
            }

            return Tiles;
        }

        private static List<string> ReadLines(string tileset)
        {
            List<string> Lines = new List<string>();
            string line;
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(ContentSettings.Content.RootDirectory + "\\" + tileset + ".txt"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        Lines.Add(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw new Exception("Tileset file was not found. Make sure every tileset picture has correctly formatted .txt file attached to it.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new Exception("Tileset directory was not found. Make sure the ListsFolder attribute is set correctly.");
            }

            return Lines;
        }

        private static Tuple<string, int> TrySplitName(string fileLine)
        {
            string[] splitted = fileLine.Split(':');
            if (splitted.Length > 0)
            {
                return new Tuple<string, int>((string)splitted[0], Int16.Parse(splitted[1]));
            }
            else
            {
                return null;
            }
        }

        private static Tuple<Rectangle, string> TrySplitSprite(string fileLine)
        {
            string[] splitted = fileLine.Split(' ');
            if (splitted.Length > 0)
            {
                return new Tuple<Rectangle, string>(new Rectangle(Int16.Parse(splitted[0]), Int16.Parse(splitted[1]), Int16.Parse(splitted[2]), Int16.Parse(splitted[3])), (string)splitted[4]);
            }
            else
            {
                return null;
            }
        }
    }
}
