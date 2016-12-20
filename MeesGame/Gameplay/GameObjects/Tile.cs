using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

        public static implicit operator Tile (TileData data)
        {
            return data.ToTile();
        }

        public static implicit operator TileData(Tile tile)
        {
            return tile.Data;
        }
    }

    public abstract class Tile : SpriteGameObject
    {
        public TileData Data
        {
            get; private set;
        }
        public Point location = Point.Zero;
        protected bool revealed = false;
        protected bool isVisited = false;
        protected SpriteSheet secondarySprite;
        protected Color secondarySpriteColor = Color.White;

        protected Tile(string assetName = "", TileType tileType = TileType.Floor, int layer = 0, string id = "") : base(assetName, layer, id)
        {
            Data = new TileData(tileType);
        }

        public bool Revealed
        {
            get
            {
                return revealed;
            }
            set
            {
                revealed = value;
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!(Parent as TileField).FogOfWar || Revealed)
            {
                base.Draw(gameTime, spriteBatch);
                if (secondarySprite != null && visible)
                {
                    secondarySprite.Draw(spriteBatch, this.GlobalPosition, origin, drawColor: secondarySpriteColor);
                }
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
        /// Updates the graphics of the tile to match the surroundings, should be called after every change in the TileField
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
            }
            //If no Tile can be made of the specified tiletype, return a floortile
            return new FloorTile();
        }

        public virtual void EnterTile(Player player) { }

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
                    return FloorTile.GetDefaultAssetNames();

                case TileType.Wall:
                    return WallTile.GetDefaultAssetNames();

                case TileType.Door:
                    return DoorTile.GetDefaultAssetNames();

                case TileType.Key:
                    return KeyTile.GetDefaultAssetNames();
            }
            //If no Tile can be made of the specified tiletype, return null
            return null;
        }

        public virtual InventoryItem GetItem()
        {
            return null;
        }

        public abstract void UpdateGraphics();

        public bool IsVisited
        {
            get
            {
                return isVisited;
            }
            set
            {
                isVisited = value;
            }
        }
    }

    class FloorTile : Tile
    {
        public const string defaultAssetName = "floorTile";

        protected FloorTile(string assetName = defaultAssetName, TileType tt = TileType.Floor, int layer = 0, string id = "") : base(assetName, tt, layer, id)
        {
        }

        public FloorTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.Floor, layer, id)
        {
        }

        public override bool CanPlayerMoveHere(Player player)
        {
            //A player is allowed to move onto floors
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


        public override void UpdateGraphics()
        {
        }

        public static string[] GetDefaultAssetNames()
        {
            return new string[] { defaultAssetName };

        }
    }

    class WallTile : Tile
    {
        public const string defaultAssetName = "walls@16";

        protected WallTile(string assetName = defaultAssetName, TileType tt = TileType.Wall, int layer = 0, string id = "") : base(assetName, tt, layer, id)
        {
        }

        public WallTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.Wall, layer, id)
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
            sprite = new SpriteSheet(defaultAssetName, sheetIndex);
        }

        public override void UpdateGraphics()
        {
        }

        public static string[] GetDefaultAssetNames()
        {
            return new string[] { defaultAssetName };
        }
    }
}
