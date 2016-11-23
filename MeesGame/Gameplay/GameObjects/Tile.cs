namespace MeesGame
{
    class Tile : SpriteGameObject
    {
        protected int TileType;
        public Tile (string assetName, int TileType, int layer = 0, string id = "") : base(assetName, layer, id)
        {
            this.TileType = TileType;
        }
    }
}
