using Microsoft.Xna.Framework;

namespace MeesGame
{
    enum TileType
    {
        Floor,
        Wall,
        Door,
        Key,
        Unknown
    }

    abstract class Tile : SpriteGameObject
    {
        protected TileType tileType;
        protected Point gridPosition;

        public Tile(string assetName = "", TileType tt = TileType.Floor, int layer = 0, string id = "") : base(assetName, layer, id)
        {
            tileType = tt;
        }

        public TileType TileType
        {
            get { return tileType; }
        }

        public Point GridPosition
        {
            get { return gridPosition; }
            set { gridPosition = value; }
        }

        /// <summary>
        /// Checks if this tile prevents a player who is currently at this Tile from performing the specified action
        /// </summary>
        /// <param name="player">The player at this Tile that wants to perform the action</param>
        /// <param name="action">The action to check</param>
        /// <returns>true if the action is forbidden by this Tile. false otherwise.</returns>
        public abstract bool IsActionForbiddenFromHere(Player player, PlayerAction action);

        /// <summary>
        /// Checks if the player can move to this tile when he is next to it.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if the player can move here. false otherwise.</returns>
        public abstract bool CanPlayerMoveHere(Player player);

        /// <summary>
        /// Updates the graphics of the tile to match the surroundings, should be called after every change in the TileField
        /// </summary>
        public abstract void UpdateGraphicsToMatchSurroundings();
    }

    class FloorTile : Tile
    {
        protected FloorTile(string assetName = "floorTile", TileType tt = TileType.Floor, int layer = 0, string id = "") : base(assetName, tt, layer, id)
        {

        }

        public FloorTile(int layer = 0, string id = "") : base("floorTile", TileType.Floor, layer, id)
        {

        }

        public override bool CanPlayerMoveHere(Player player)
        {
            //a player is allowed to move onto floors
            return true;
        }

        public override bool IsActionForbiddenFromHere(Player player, PlayerAction action)
        {
            //A player can move into all directions from a floor tile.
            //Special actions are allowed on floor tiles, if there is a tile which allows a special action next to it.
            if (IsNextToSpecialTile())
            {
                return false;
            }
            //Special actions are not allowed on a floor tile, if it is surrounded by tiles that don't allow special actions. 
            else
                return action == PlayerAction.SPECIAL;
        }

        public override void UpdateGraphicsToMatchSurroundings()
        {
        }

        //TODO
        public bool IsNextToSpecialTile()
        {
            return false;
        }
    }

    class WallTile : Tile
    {
        protected WallTile(string assetName = "walls@16", TileType tt = TileType.Wall, int layer = 0, string id = "") : base(assetName, tt, layer, id)
        {

        }

        public WallTile(int layer = 0, string id = "") : base("walls@16", TileType.Wall, layer, id)
        {

        }

        public override bool CanPlayerMoveHere(Player player)
        {
            //A player can never walk onto a wall
            return false;
        }

        public override bool IsActionForbiddenFromHere(Player player, PlayerAction action)
        {
            //A player should never even be on a Wall Tile
            return true;
        }

        public override void UpdateGraphicsToMatchSurroundings()
        {
            int sheetIndex = 0;
            int x = gridPosition.X;
            int y = gridPosition.Y;
            TileField tileField = Parent as TileField;
            if (tileField.GetTile(x, y - 1) is WallTile) sheetIndex += 1;
            if (tileField.GetTile(x + 1, y) is WallTile) sheetIndex += 2;
            if (tileField.GetTile(x, y + 1) is WallTile) sheetIndex += 4;
            if (tileField.GetTile(x - 1, y) is WallTile) sheetIndex += 8;
            sprite = new SpriteSheet("walls@16", sheetIndex);
        }
    }
}
