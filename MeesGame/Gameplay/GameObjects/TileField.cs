using System;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class TileField : GameObjectGrid
    {
        public TileField(int rows, int columns, int layer = 0, string id = "") : base(rows, columns, layer, id)
        {

        }

        public TileType GetType(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Columns || y >= Rows)
            {
                return TileType.Wall;
            }
            Tile tile = Get(x, y) as Tile;
            return tile.TileType;
        }
    }

    class TileFieldView : GameObjectList
    {
        TileField tileField;
        Player pov;
        public TileFieldView(Player pov, TileField tileField, string id = "")
        {
            Add(tileField);
            Add(pov);
            this.pov = pov;
            this.tileField = tileField;
        }

        public bool IsVisible(Point location)
        {
            return IsVisible(location.X, location.Y);
        }

        public bool IsVisible(int x, int y)
        {
            //for now, this function will always return true
            //we might change this behaviour later
            return true;
        }

        public Tile Get(int x, int y)
        {
            if (IsVisible(x, y))
            {
                return (Tile)tileField.Get(x, y);
            }
            else
            {
                return null;
            }
        }

        public TileType GetType(Point location)
        {
            if (IsVisible(location))
            {
                return tileField.GetType(location.X, location.Y);
            }
            else
            {
                return TileType.Unknown;
            }
        }

        public Vector2 GetAnchorPosition(GameObject obj)
        {
            return tileField.GetAnchorPosition(obj);
        }

        public Vector2 GetAnchorPosition(Point location)
        {
            return tileField.GetAnchorPosition(location);
        }
    }
}
