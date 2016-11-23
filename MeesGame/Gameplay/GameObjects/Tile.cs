namespace MeesGame
{
    enum TileType
    {
        floor,
        wall
    }

    class Tile : SpriteGameObject
    {
        protected TileType tileType;

        public Tile (string assetName = "", TileType tt = TileType.floor, int layer = 0, string id = "") : base(assetName, layer, id)
        {
            tileType = tt;
        }

        public TileType TileType
        {
            get { return TileType; }
        }
    }
}
