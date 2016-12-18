using Microsoft.Xna.Framework;
using System;

namespace MeesGame
{
    enum TileType
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

        //TODO
        public bool IsNextToSpecialTile()
        {
            return false;
        }
    }

    class WallTile : Tile
    {
        protected WallTile(string assetName = "fourWayWallTile", TileType tt = TileType.Wall, int layer = 0, string id = "") : base(assetName, tt, layer, id)
        {

        }

        public WallTile(int layer = 0, string id = "") : base("fourWayWallTile", TileType.Wall, layer, id)
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
    }
}
