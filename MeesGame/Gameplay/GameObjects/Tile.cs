using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    public abstract class Tile : SpriteGameObject
    {
        public TileData Data
        {
            get; set;
        }

        public Point Location
        {
            get; set;
        } = Point.Zero;

        protected int revealed = 0;
        protected bool obstructsVision;

        public bool IsVisited
        {
            get; private set;
        }

        protected SpriteSheet fog = new SpriteSheet("fog@16");
        protected SpriteSheet secondarySprite;

        protected Color secondarySpriteColor = Color.White;

        protected Tile() : base("")
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

        public override Vector2 Position
        {
            get
            {
                return new Vector2(Location.X * TileField.CellWidth, Location.Y * TileField.CellHeight);
            }
        }

        public virtual void EnterCenterOfTile(ITileFieldPlayer player) { }

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
        public abstract bool IsActionForbiddenFromHere(ITileFieldPlayer player, PlayerAction action);

        /// <summary>
        /// Checks if the player can move to this tile when he is next to it.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if the player can move here. false otherwise.</returns>
        public abstract bool CanPlayerMoveHere(ITileFieldPlayer player);

        /// <summary>
        /// The location that the player will be on after performing the specified action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual Point GetLocationAfterAction(PlayerAction action)
        {
            Point newLocation = this.Location;
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
        /// Perform an action for the player. Modifies the ITileFieldPlayer appropriately
        /// </summary>
        /// <param name="player">The player that is performing the action</param>
        /// <param name="action">The action to perform</param>
        public virtual void PerformAction(ITileFieldPlayer player, PlayerAction action)
        {
            player.LastAction = action;
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

        /// <summary>
        /// The four direct neighbors of this Tile
        /// </summary>
        public ICollection<Tile> Neighbours
        {
            get
            {
                List<Tile> neighbours = new List<Tile>();
                foreach (PlayerAction action in DummyPlayer.MOVEMENT_ACTIONS)
                {
                    neighbours.Add(TileField.GetTile(GetLocationAfterAction(action)));
                }
                return neighbours;
            }
        }

        /// <summary>
        /// Updates the graphics of the tile to match the surroundings, should be called after every change in the TileField.
        /// </summary>
        public abstract void UpdateGraphicsToMatchSurroundings();

        /// <summary>
        /// Updates the information of the portals to set the destination after a special action.
        /// </summary>
        public virtual void UpdatePortals()
        {
        }

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

                case TileType.Portal:
                    return new PortalTile();

                case TileType.Ice:
                    return new IceTile();

                case TileType.Guard:
                    return new GuardTile();
            }
            //If no Tile can be made of the specified tiletype, return a floortile
            return new FloorTile();
        }

        public virtual void EnterTile(ITileFieldPlayer player)
        {
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
                    return new string[] { "horizontalEnd" };

                case TileType.Start:
                    return new string[] { "start" };

                case TileType.Portal:
                    return new string[] { "ladder" };

                case TileType.Ice:
                    return new string[] { "floorTile", "Ice" };

                case TileType.Guard:
                    return new string[] { "guard" };
            }
            //If no Tile can be made of the specified tiletype, return null
            return null;
        }

        /// <summary>
        /// Updates the graphics of the tile according to things happening while playing.
        /// </summary>
        public abstract void UpdateGraphics();

        public virtual void UpdateToAdditionalInfo() { }

        public virtual void MarkVisited()
        {
            IsVisited = true;
        }

        public virtual bool StopsSliding
        {
            get
            {
                return false;
            }
        }
    }
}
