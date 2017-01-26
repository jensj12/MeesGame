using MeesGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AI
{
    /// <summary>
    /// TileTypes the AStart algoritm can encounter.
    /// </summary>
    enum AStarTileType
    {
        passable, blocked, ice, target, portal, end
    }

    /// <summary>
    /// Reference to the location of a tile which requires a color to be passed, or gives a color.
    /// </summary>
    struct ColoredTile
    {
        /// <summary>
        /// Location of the referenced tile.
        /// </summary>
        public Point TileLocation
        {
            get;
        }

        /// <summary>
        /// Color of the referenced tile.
        /// </summary>
        public KeyColor Color
        {
            get;
        }

        public ColoredTile(Point tileLocation, KeyColor color)
        {
            TileLocation = tileLocation;
            Color = color;
        }
    }

    class AStarTile
    {
        /// <summary>
        /// Location as in the multidimensional array.
        /// </summary>
        public Point Location
        {
            get;
        }

        /// <summary>
        /// Action that is required to enter this tile from the previous tile.
        /// </summary>
        public PlayerAction? ActionToEnter
        {
            private set; get;
        }

        /// <summary>
        /// Tile from which this tile was accessed.
        /// </summary>
        public AStarTile PreviousTile
        {
            private set; get;
        }

        /// <summary>
        /// Amount of time it takes to reach this tile. Lower is better.
        /// </summary>
        public long Score
        {
            get; set;
        }

        public AStarTileType TileType;

        /// <summary>
        /// If standing on this tile kills the player.
        /// </summary>
        public bool Kills = false;

        /// <summary>
        /// Location of the tile this portal connects with.
        /// </summary>
        public Point Destination;

        /// <summary>
        /// Sets which tile was entered before this tile and what action the player has to take when standing on that tile.
        /// </summary>
        /// <param name="previousTile">The tile which was previously entered</param>
        /// <param name="x">X direction to get to this tile.</param>
        /// <param name="y">Y direction to get to this tile.</param>
        public void SetOldTile(AStarTile previousTile, int x, int y)
        {
            this.PreviousTile = previousTile;

            if (x == 0 && y == 0)
                ActionToEnter = PlayerAction.SPECIAL;
            else if (x != 0)
                if (x > 0)
                    ActionToEnter = PlayerAction.EAST;
                else ActionToEnter = PlayerAction.WEST;
            else if (y > 0)
                ActionToEnter = PlayerAction.SOUTH;
            else ActionToEnter = PlayerAction.NORTH;
        }

        public AStarTile(Point location, AStarTileType tileType, bool kills = false, Point? destination = null, long score = long.MaxValue)
        {
            ActionToEnter = null;
            Score = score;
            Kills = kills;
            Destination = destination ?? Point.Zero;
            Location = location;
            TileType = tileType;
        }

        /// <summary>
        /// Clones the tile, excludes the score.
        /// </summary>
        public AStarTile Clone
        {
            get { return new AStarTile(Location, TileType, Kills); }
        }
    }

    /// <summary>
    /// Contains the level that needs to be solved. Every different obtained key creates a new level if that a key of the same color wasn't present before.
    /// </summary>
    class AStarLevel
    {
        /// <summary>
        /// Contains the tiles and their scores.
        /// </summary>
        internal AStarTile[,] Field;

        /// <summary>
        /// Tiles that have been reached by another tile, but have not yet been tested themselves.
        /// </summary>
        internal List<AStarTile> OpenTiles;

        /// <summary>
        /// Tiles which contain a key of a color that hasn't been picked up yet.
        /// </summary>
        internal HashSet<ColoredTile> PotentialKeys;

        /// <summary>
        /// Tiles which contain a locked door.
        /// </summary>
        internal HashSet<ColoredTile> ClosedDoors;

        /// <summary>
        /// Keys that have been reached while solving this level, and can thus be used as a starting point next level.
        /// </summary>
        internal HashSet<ColoredTile> ReachedKeys;

        /// <summary>
        /// The finish tile, null if it hasn't been reached yet.
        /// </summary>
        internal AStarTile Finish = null;

        /// <summary>
        /// Tile from which the level started.
        /// </summary>
        internal AStarTile Start;

        /// <summary>
        /// The path that requires the least amount of actions to reach the finish or the next key leading to the finish.
        /// </summary>
        public Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>> OptimalPath;

        /// <summary>
        /// Keys which have been reached while solving the level.
        /// </summary>
        private HashSet<KeyColor> obtainedKeys;

        public AStarLevel(TileField tileField)
        {
            OpenTiles = new List<AStarTile>();
            PotentialKeys = new HashSet<ColoredTile>();
            ReachedKeys = new HashSet<ColoredTile>();
            ClosedDoors = new HashSet<ColoredTile>();
            obtainedKeys = new HashSet<KeyColor>();
            OptimalPath = new Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>>(HashSet<KeyColor>.CreateSetComparer());

            Field = new AStarTile[tileField.Objects.GetLength(0), tileField.Objects.GetLength(1)];

            for (int x = 0; x < tileField.Objects.GetLength(0); x++)
            {
                for (int y = 0; y < tileField.Objects.GetLength(1); y++)
                {
                    Tile tileAtLocation = ((Tile)tileField.Objects[x, y]);
                    switch (tileAtLocation.TileType)
                    {
                        case TileType.Floor:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.passable);
                            break;
                        case TileType.Wall:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.blocked);
                            break;
                        case TileType.Ice:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.ice);
                            break;
                        case TileType.End:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.end);
                            break;
                        case TileType.Start:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.passable, score: 0);
                            Start = Field[x, y];
                            OpenTiles.Add(Field[x, y]);
                            break;
                        case TileType.Key:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.target);
                            PotentialKeys.Add(new ColoredTile(new Point(x, y), ((KeyTile)tileAtLocation).KeyColor));
                            break;
                        case TileType.Door:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.blocked);
                            ClosedDoors.Add(new ColoredTile(new Point(x, y), ((DoorTile)tileAtLocation).DoorColor));
                            break;
                        case TileType.Portal:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.portal, destination: ((PortalTile)tileAtLocation).Destination);
                            break;
                        default:
                            Field[x, y] = new AStarTile(new Point(x, y), AStarTileType.blocked, true);
                            break;
                    }
                }
            }
        }

        public AStarLevel(AStarLevel oldField, ColoredTile newKey, long score)
        {
            OpenTiles = new List<AStarTile>();
            PotentialKeys = new HashSet<ColoredTile>();
            ReachedKeys = new HashSet<ColoredTile>();
            ClosedDoors = new HashSet<ColoredTile>();
            obtainedKeys = new HashSet<KeyColor>();
            OptimalPath = new Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>>();

            Field = new AStarTile[oldField.Field.GetLength(0), oldField.Field.GetLength(1)];
            for (int x = 0; x < Field.GetLength(0); x++)
                for (int y = 0; y < Field.GetLength(1); y++)
                    Field[x, y] = oldField.Field[x, y].Clone;

            Field[newKey.TileLocation.X, newKey.TileLocation.Y] = new AStarTile(newKey.TileLocation, AStarTileType.passable, score: score);
            Start = Field[newKey.TileLocation.X, newKey.TileLocation.Y];
            OpenTiles.Add(Field[newKey.TileLocation.X, newKey.TileLocation.Y]);

            //Copy keys exept the reached key
            foreach (ColoredTile key in oldField.PotentialKeys)
                if (key.Color != newKey.Color)
                    PotentialKeys.Add(key);
                else
                    Field[key.TileLocation.X, key.TileLocation.Y].TileType = AStarTileType.passable;

            //Copy doors exept the doors of the color of the new key.
            foreach (ColoredTile door in oldField.ClosedDoors)
                if (door.Color != newKey.Color)
                    PotentialKeys.Add(door);
                else
                    Field[door.TileLocation.X, door.TileLocation.Y].TileType = AStarTileType.passable;

            foreach (KeyColor key in oldField.obtainedKeys)
                obtainedKeys.Add(key);
            obtainedKeys.Add(newKey.Color);
        }

        /// <summary>
        /// Solves the level.
        /// </summary>
        public void SolveLevel()
        {
            while (OpenTiles.Count > 0)
            {
                OpenTiles.Sort((t1, t2) => t1.Score.CompareTo(t2.Score));
                VisitTile(OpenTiles[0]);
            }

            HashSet<AStarLevel> innerFields = new HashSet<AStarLevel>();

            AStarLevel optimalField = null;

            foreach (ColoredTile t in ReachedKeys)
            {
                AStarLevel newField = new AStarLevel(this, t, Field[t.TileLocation.X, t.TileLocation.Y].Score);
                innerFields.Add(newField);
                newField.SolveLevel();
                if (newField.Finish?.Score < (Finish?.Score ?? long.MaxValue))
                {
                    Finish = newField.Finish;
                    optimalField = newField;
                }
            }

            if (optimalField != null)
            {
                OptimalPath.Add(obtainedKeys, CreateOptimalPath(Start, Field[(int)optimalField?.Start.Location.X, (int)optimalField?.Start.Location.Y]));
                foreach (HashSet<KeyColor> keys in optimalField.OptimalPath.Keys)
                    OptimalPath.Add(keys, optimalField.OptimalPath[keys]);
            }
            else if (Finish != null)
                OptimalPath.Add(obtainedKeys, CreateOptimalPath(Start, Field[(int)Finish?.Location.X, (int)Finish?.Location.Y]));
        }

        /// <summary>
        /// Creates the optimal path from the Field based on which tile is started at and at which tile should be ended.
        /// </summary>
        /// <param name="start">Tile from which path begins.</param>
        /// <param name="end">Tile at which the path ends.</param>
        /// <returns></returns>
        private Dictionary<Point, PlayerAction> CreateOptimalPath(AStarTile start, AStarTile end)
        {
            Dictionary<Point, PlayerAction> optimalPath = new Dictionary<Point, PlayerAction>();
            AStarTile currentTile = end;
            while (currentTile != start)
            {
                optimalPath.Add(currentTile.PreviousTile.Location, (PlayerAction)currentTile.ActionToEnter);
                currentTile = currentTile.PreviousTile;
            }
            return optimalPath;
        }

        /// <summary>
        /// Checks which tile the tile connects to and removes the tile from the OpenTiles after it is finished.
        /// </summary>
        /// <param name="tile"></param>
        private void VisitTile(AStarTile tile)
        {
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                    if ((dx + dy) * (dx + dy) == 1)
                        TestTileInDirection(tile, dx, dy);
            OpenTiles.Remove(tile);
        }

        /// <summary>
        /// Test which tile is pointed at and deals with the tile accordingly.
        /// </summary>
        /// <param name="startTile">Tile from which the new tile is entered</param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        private void TestTileInDirection(AStarTile startTile, int dx, int dy)
        {
            if (startTile.Location.X + dx > -1 && startTile.Location.Y + dy > -1 && startTile.Location.X + dx < Field.GetLength(0) && startTile.Location.Y + dy < Field.GetLength(1))
            {
                AStarTile newTile = Field[startTile.Location.X + dx, startTile.Location.Y + dy];
                //score +1 to check if not out of range.
                if (newTile.Score > startTile.Score + 1 && newTile.TileType != AStarTileType.blocked && startTile.Score + 1 > 0)
                    switch (newTile.TileType)
                    {
                        case AStarTileType.end:
                            newTile.Score = startTile.Score + 1;
                            newTile.SetOldTile(startTile, dx, dy);
                            if (!(Finish?.Score > newTile.Score))
                                Finish = newTile;
                            break;
                        case AStarTileType.ice:
                            AStarTile nextTile = AStar.Level.Field[startTile.Location.X + NextIncrementInSameDirection(dx), startTile.Location.Y + NextIncrementInSameDirection(dy)];
                            if (!nextTile.Kills)
                                if (nextTile.TileType == AStarTileType.blocked || nextTile.TileType == AStarTileType.end)
                                    goto case AStarTileType.passable;
                                else
                                    TestTileInDirection(startTile, NextIncrementInSameDirection(dx), NextIncrementInSameDirection(dy));
                            break;
                        case AStarTileType.passable:
                            newTile.Score = startTile.Score + 1;
                            newTile.SetOldTile(startTile, dx, dy);
                            OpenTiles.Add(newTile);
                            break;
                        case AStarTileType.target:
                            newTile.Score = startTile.Score + 1;
                            foreach (ColoredTile ct in PotentialKeys)
                                if (ct.TileLocation == newTile.Location)
                                {
                                    ReachedKeys.Add(ct);
                                    break;
                                }
                            newTile.SetOldTile(startTile, dx, dy);
                            break;
                        case AStarTileType.portal:
                            newTile.Score = startTile.Score + 1;
                            AStarTile destination = Field[newTile.Destination.X, newTile.Destination.Y];
                            if (newTile.Score + 1 < destination.Score)
                            {
                                destination.Score = newTile.Score + 1;
                                destination.SetOldTile(newTile, 0, 0);
                            }
                            OpenTiles.Add(destination);
                            goto case AStarTileType.passable;
                    }
            }
        }

        /// <summary>
        /// Increments the parameter i by +1 or -1.
        /// </summary>
        /// <param name="i">Number to be increased or decreased by 1.</param>
        /// <returns></returns>
        private int NextIncrementInSameDirection(int i)
        {
            return i + ((i == 0) ? 0 : ((i > 0) ? +1 : -1));
        }
    }

    class AStar : IAI
    {
        IAIPlayer player;

        /// <summary>
        /// The level as before the AI starts moving.
        /// </summary>
        public static AStarLevel Level
        {
            get; set;
        }

        /// <summary>
        /// Path based on the location of the tile the player is standing on and the keys the player has in his inventory.
        /// </summary>
        private Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>> path;

        public void GameStart(IAIPlayer player, int difficulty)
        {
            this.player = player;
            path = FindPath();
        }

        public void ThinkAboutNextAction()
        {
        }

        /// <summary>
        /// Finds a path. Throws PathNotFoundException when no path can be found.
        /// </summary>
        /// <returns>A finished path.</returns>
        private Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>> FindPath()
        {
            Level = new AStarLevel(player.TileField);
            Level.SolveLevel();
            if (Level.OptimalPath.Count == 0)
                throw new PathNotFoundException();
            return Level.OptimalPath;
        }

        public void UpdateNextAction()
        {
            HashSet<KeyColor> currentKeys = new HashSet<KeyColor>();
            foreach (InventoryItem item in player.DummyPlayer.Inventory.Items)
                currentKeys.Add(item.type.ToKeyColorType());
            Dictionary<Point, PlayerAction> CurrentPath = path[currentKeys];
            if (CurrentPath.ContainsKey(player.DummyPlayer.Location))
                player.NextAIAction = CurrentPath[player.DummyPlayer.Location];
        }
    }

    /// <summary>
    /// Error message thrown when no path has been found.
    /// </summary>
    public class PathNotFoundException : Exception
    {
    }
}
