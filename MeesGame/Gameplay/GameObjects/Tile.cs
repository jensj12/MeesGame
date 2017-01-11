using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;

namespace MeesGame
{
    public abstract class Tile : SpriteGameObject
    {
        public TileData Data
        {
            get; private set;
        }

        public Point location = Point.Zero;
        protected int revealed = 0;
        protected bool obstructsVision;

        public bool IsVisited
        {
            get; private set;
        }

        protected SpriteSheet fog = new SpriteSheet("fog@16");
        protected SpriteSheet secondarySprite;

        protected Color secondarySpriteColor = Color.White;

        public Tile() : base("")
        {
        }

        protected Tile(TileType tileType = TileType.Floor, int layer = 0, string id = "") : base(GetAssetNamesFromTileType(tileType)[0], layer, id)
        {
            Data = new TileData(tileType);
            if (GetAssetNamesFromTileType(tileType).Length >= 2)
                secondarySprite = new SpriteSheet(GetAssetNamesFromTileType(tileType)[1]);
        }

        public int Revealed
        {
            set
            {
                revealed |= value;
            }
        }

        public bool ObstructsVision
        {
            get
            {
                return obstructsVision;
            }
        }

        /// <summary>
        /// The type of Tile
        /// </summary>
        public TileType TileType
        {
            get { return Data.TileType; }
        }

        /// <summary>
        /// The location on the TileField
        /// </summary>
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        public override Vector2 Position
        {
            get
            {
                return new Vector2(Location.X * TileField.CellWidth, Location.Y * TileField.CellHeight);
            }
        }

        public virtual void EnterCenterOfTile(Player player) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (secondarySprite != null && visible)
            {
                secondarySprite.Draw(spriteBatch, this.GlobalPosition, origin, drawColor: secondarySpriteColor);
            }

            if ((parent as TileField).FogOfWar)
            {
                fog.SheetIndex = revealed;
                fog.Draw(spriteBatch, this.GlobalPosition, origin);
            }
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
        /// Perform an action for the player. Modifies the Player appropriately
        /// </summary>
        /// <param name="player">The player that is performing the action</param>
        /// <param name="action">The action to perform</param>
        public virtual void PerformAction(Player player, PlayerAction action)
        {
            if (action.IsDirection())
                player.MoveSmoothly(action.ToDirection());
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

        [Editor]
        public Color SecondarySpriteColor
        {
            get { return secondarySpriteColor; }
            set { secondarySpriteColor = value; }
        }

        /// <summary>
        /// The four direct neighbors of this Tile
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

        public bool IsNextToSpecialTile()
        {
            foreach (Tile tile in Neighbours)
            {
                if (tile.TileType == TileType.Door)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the graphics of the tile to match the surroundings, should be called after every change in the TileField.
        /// </summary>
        public abstract void UpdateGraphicsToMatchSurroundings();

        /// <summary>
        /// Used to create Tiles solemnly on a tileType
        /// </summary>
        /// <param name="tt">Specifies which Tile we want to get</param>
        /// <returns></returns>
        public static Tile CreateTileFromTileType(TileType tt)
        {
            switch (tt)
            {
                case TileType.Floor:
                    return new FloorTile();

                case TileType.Wall:
                    return new WallTile();

                case TileType.Door:
                    return new DoorTile();

                case TileType.Key:
                    return new KeyTile();

                case TileType.Start:
                    return new StartTile();

                case TileType.End:
                    return new EndTile();

                case TileType.Hole:
                    return new HoleTile();
            }
            //If no Tile can be made of the specified tiletype, return a floortile
            return new FloorTile();
        }

        public virtual void EnterTile(Player player)
        {
            IsVisited = true;
        }

        public static Tile CreateTileFromTileData(TileData data)
        {
            return CreateTileFromTileType(data.TileType);
        }

        /// <summary>
        /// Returns the default asset name of the specified tileType. If the specified tileType doesn't have a default assetName, returns ""
        /// </summary>
        /// <param name="tt"></param>
        /// <returns></returns>
        public static string[] GetAssetNamesFromTileType(TileType tt)
        {
            switch (tt)
            {
                case TileType.Floor:
                    return new string[] { "floorTile" };

                case TileType.Wall:
                    return new string[] { "walls@16" };

                case TileType.Door:
                    return new string[] { "horizontalDoor", "horizontalDoorOverlay" };

                case TileType.Key:
                    return new string[] { "floorTile", "keyOverlay" };

                case TileType.Hole:
                    return new string[] { "hole" };

                case TileType.End:
                    return new string[] { "floorTile", "money" };

                case TileType.Start:
                    return new string[] { "ladder" };

            }
            //If no Tile can be made of the specified tiletype, return null
            return null;
        }

        /// <summary>
        /// Updates the graphics of the tile according to things happening while playing.
        /// </summary>
        public abstract void UpdateGraphics();
    }
}
