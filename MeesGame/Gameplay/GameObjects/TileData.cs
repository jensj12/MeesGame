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
        Ice,
        Unknown
    }

    public struct TileData
    {
        public TileType TileType { get; set; }
        public int AdditionalInfo { get; set; }

        public TileData(TileType tileType, int additionalInfo = 0)
        {
            TileType = tileType;
            AdditionalInfo = additionalInfo;
        }

        public Tile ToTile()
        {
            Tile tile = Tile.CreateTileFromTileType(TileType);
            tile.Data = this;
            tile.UpdateToAdditionalInfo();
            return tile;
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
