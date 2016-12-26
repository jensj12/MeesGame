namespace MeesGame
{
    public enum TileType
    {
        Floor,
        Wall,
        Door,
        Key,
        Start,
        End,
        Hole,
        Unknown
    }

    public struct TileData
    {
        public TileType TileType { get; set; }
        public TileData(TileType tileType)
        {
            TileType = tileType;
        }

        public Tile ToTile()
        {
            return Tile.CreateTileFromTileType(TileType);
        }

        public static implicit operator Tile(TileData data)
        {
            return data.ToTile();
        }

        public static implicit operator TileData(Tile tile)
        {
            return tile.Data;
        }
    }
}
