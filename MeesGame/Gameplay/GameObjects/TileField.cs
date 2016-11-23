namespace MeesGame
{
    class TileField : GameObjectGrid
    {
        public TileField(int rows, int columns, int layer = 0, string id = "") : base(rows, columns, layer, id)
        {

        }

        public TileType GetType(int x, int y)
        {
            if (x < 0 || y < 0 || x > Columns || y > Rows)
            {
                return TileType.wall;
            }
            Tile tile = Get(x, y) as Tile;
            return tile.TileType;
        }
    }
}
