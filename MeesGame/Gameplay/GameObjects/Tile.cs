using MeesGame.Gameplay.GameObjects.Tiles;

namespace MeesGame
{
    enum TileType
    {
        Floor,
        Wall,
        Unknown
    }

    abstract class Tile : SpriteGameObject
    {
        public abstract string[] textureNames { get; }

        protected TileType tileType;

        public Tile (string assetName = "", int layer = 0, string id = "") : base(assetName, layer, id)
        {

        }

        public TileType TileType
        {
            get { return tileType; }
        }

        public static Tile GetTileFromTileType(TileType tt)
        {
            switch (tt)
            {
                case TileType.Floor:
                    return new FloorTile();
                case TileType.Wall:
                    return new WallTile();    
            }
            return new UnknownTile();
        }

        public virtual SpriteSheet GetSpriteSheet()
        {
            return new SpriteSheet(textureNames[0]);
        }

        public static SpriteSheet GetDefaultSpriteSheetFromType(TileType tt)
        {
            return GetTileFromTileType(tt).GetSpriteSheet();
        }
    }
}
