using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesGame.Gameplay.GameObjects.Tiles
{
    class WallTile : Tile
    {
        bool left, right, bottom, top;

        public override string[] textureNames
        {
            get
            {
                return new string[]{
                    "fourWayWallTile",
                    "threeWayWallTile",
                    "straightWallTile",
                    "cornerWallTile"
                };
            }
        }

        public WallTile() : base("fourWayWallTile")
        {
        }

        public void UpdateAdjacentTiles(GameObject[,] map)
        {

            int x = 0, y = 0;

            bool found = false;

             for (x = 0; x < map.GetLength(0); x++)
            {
                for (y = 0; y < map.GetLength(1) && !found; y++)
                {
                    if (map[x, y] == this)
                    {
                        found = true;
                        break;
                    }
                }
            }

            left = ((Tile)(map[x - 1, y])).TileType == TileType.Floor;
            right = ((Tile)(map[x + 1, y])).TileType == TileType.Floor;
            bottom = ((Tile)(map[x, y - 1])).TileType == TileType.Floor;
            top = ((Tile)(map[x, y + 1])).TileType == TileType.Floor;

            sprite = GetSpriteSheet();
        }

        public override SpriteSheet GetSpriteSheet()
        {

            if (bottom && top && left && right)
                return new SpriteSheet(textureNames[0], 0, 0f);

            if (bottom && right && top)
                return new SpriteSheet(textureNames[1], 0, 0f);

            if (left && bottom && right)
                return new SpriteSheet(textureNames[1], 0, 90f);

            if (top && left && bottom)
                return new SpriteSheet(textureNames[1], 0, 180f);

            if (left && top && right)
                return new SpriteSheet(textureNames[1], 0, 270f);

            if (top && bottom)
                return new SpriteSheet(textureNames[1], 0, 0f);

            if (left && right)
                return new SpriteSheet(textureNames[1], 0, 90f);

            if (bottom && right)
                return new SpriteSheet(textureNames[2], 0, 0f);

            if (left && bottom)
                return new SpriteSheet(textureNames[2], 0, 90f);

            if (top && left)
                return new SpriteSheet(textureNames[2], 0, 180f);

            if (right && top)
                return new SpriteSheet(textureNames[2], 0, 270f);

            if (top)
                return new SpriteSheet(textureNames[3], 0, 0f);

            if (right)
                return new SpriteSheet(textureNames[3], 0, 90f);

            if (bottom)
                return new SpriteSheet(textureNames[3], 0, 180f);

            if (left)
                return new SpriteSheet(textureNames[3], 0, 270f);

            //we just return the cornerwall as a default texture;
            return new SpriteSheet(textureNames[3], 0, 0f);


        }
    }
}
