using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
        protected Point location = Point.Zero;

        protected Tile(string assetName = "", TileType tileType = TileType.Floor, int layer = 0, string id = "") : base(assetName, layer, id)
        {
            this.tileType = tileType;
        }

        /// <summary>
        /// The type of Tile
        /// </summary>
        public TileType TileType
        {
            get { return tileType; }
        }

        /// <summary>
        /// The location on the TileField
        /// </summary>
        public Point Location
        {
            get { return location; }
            set { location = value; }
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
        /// The location that the player will be on after performing the specified action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Point GetLocationAfterAction(PlayerAction action)
        {
            Point newLocation = this.location;
            switch (action)
            {
                case PlayerAction.NORTH:
                    newLocation.Y--;
                    break;
                case PlayerAction.EAST:
                    newLocation.X++;
                    break;
                case PlayerAction.SOUTH:
                    newLocation.Y++;
                    break;
                case PlayerAction.WEST:
                    newLocation.X--;
                    break;
            }
            return newLocation;
        }

        /// <summary>
        /// Perform an action for the player. Modifies the PlayerState appropriately
        /// </summary>
        /// <param name="player">The player that is performing the action</param>
        /// <param name="state">The state of the player to be modified by this function</param>
        /// <param name="action">The action to perform</param>
        public virtual void PerformAction(Player player, PlayerState state, PlayerAction action)
        {
            if (action.IsDirection())
                state.Character.MoveSmoothly(action.ToDirection());
        }

        /// <summary>
        /// The TileField this tile is in
        /// </summary>
        public TileField TileField
        {
            get
            {
                return (TileField)parent;
            }

            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// The four direct neighbours of this Tile
        /// </summary>
        public ICollection<Tile> Neighbours
        {
            get
            {
                List<Tile> neighbours = new List<Tile>();
                foreach (PlayerAction action in Player.MOVEMENT_ACTIONS)
                {
                    neighbours.Add(TileField.GetTile(GetLocationAfterAction(action)));
                }
                return neighbours;
            }
        }

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
            int x = Location.X;
            int y = Location.Y;
            TileField tileField = Parent as TileField;
            if (tileField.GetTile(x, y - 1) is WallTile) sheetIndex += 1;
            if (tileField.GetTile(x + 1, y) is WallTile) sheetIndex += 2;
            if (tileField.GetTile(x, y + 1) is WallTile) sheetIndex += 4;
            if (tileField.GetTile(x - 1, y) is WallTile) sheetIndex += 8;
            sprite = new SpriteSheet("walls@16", sheetIndex);
        }
    }
}
