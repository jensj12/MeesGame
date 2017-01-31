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

    class AStarTile
    {
        private readonly Tile tile;

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

        /// <summary>
        /// Location as in the multidimensional array.
        /// </summary>
        public Point Location
        {
            get { return tile.Location; }
        }

        public AStarTileType TileType(HashSet<KeyColor> inventoryKeys)
        {
            switch (tile.TileType)
            {
                case MeesGame.TileType.Floor:
                    return AStarTileType.passable;
                case MeesGame.TileType.Wall:
                    return AStarTileType.blocked;
                case MeesGame.TileType.Ice:
                    return AStarTileType.ice;
                case MeesGame.TileType.End:
                    return AStarTileType.end;
                case MeesGame.TileType.Start:
                    return AStarTileType.passable;
                case MeesGame.TileType.Key:
                    if (inventoryKeys != null)
                        foreach (KeyColor key in inventoryKeys)
                            if ((tile as KeyTile).KeyColor == key)
                                return AStarTileType.passable;
                    return AStarTileType.target;

                case MeesGame.TileType.Door:
                    if(inventoryKeys != null)
                        foreach (KeyColor key in inventoryKeys)
                            if ((tile as DoorTile).DoorColor == key)
                                return AStarTileType.passable;
                    return AStarTileType.blocked;
                case MeesGame.TileType.Portal:
                    return AStarTileType.portal;
                default:
                    return AStarTileType.blocked;
            }
        }

        /// <summary>
        /// If standing on this tile kills the player.
        /// </summary>
        public bool Kills
        {
            get { return tile.TileType == MeesGame.TileType.Guard || tile.TileType == MeesGame.TileType.Hole; }
        }

        /// <summary>
        /// Location of the tile this portal connects with.
        /// </summary>
        public Point Destination
        {
            get { return (tile as PortalTile).Destination; }
        }

        /// <summary>
        /// Color of the referenced tile.
        /// </summary>
        public KeyColor Color
        {
            get { return (tile as KeyTile).KeyColor; }
        }

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

        public AStarTile(Tile tile, long score = long.MaxValue)
        {
            this.tile = tile;
            Score = score;
        }

        /// <summary>
        /// Clones the tile, excludes the score.
        /// </summary>
        public AStarTile Clone
        {
            get { return new AStarTile(tile); }
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
        /// Keys that have been reached while solving this level, and can thus be used as a starting point next level.
        /// </summary>
        internal HashSet<AStarTile> ReachedKeys;

        /// <summary>
        /// The finish tile, null if it hasn't been reached yet.
        /// </summary>
        internal AStarTile Finish = null;

        /// <summary>
        /// AStarLevel that contains the optimal path from this level.
        /// </summary>
        internal AStarLevel optimalField = null;

        /// <summary>
        /// Tile from which the level started.
        /// </summary>
        private AStarTile start;

        internal AStarTile Start
        {
            get { return start; }
            set {
                start = value;
                OpenTiles.Add(start);
            }
        }

        public AStarLevel(TileField tileField)
        {
            OpenTiles = new List<AStarTile>();
            ReachedKeys = new HashSet<AStarTile>();
            obtainedKeys = new HashSet<KeyColor>();

            Field = new AStarTile[tileField.Objects.GetLength(0), tileField.Objects.GetLength(1)];

            for (int x = 0; x < tileField.Objects.GetLength(0); x++)
            {
                for (int y = 0; y < tileField.Objects.GetLength(1); y++)
                {
                    Tile tileAtLocation = tileField.GetTile(x, y);
                    AStarTile convertedTile;
                    if (tileAtLocation.TileType == TileType.Start)
                    {
                        convertedTile = new AStarTile(tileAtLocation, 0);
                        Start = convertedTile;
                    }
                    else
                        convertedTile = new AStarTile(tileAtLocation);
                    Field[x, y] = convertedTile;
                }
            }
        }

        public AStarLevel(AStarLevel oldField, AStarTile newKey)
        {
            OpenTiles = new List<AStarTile>();
            ReachedKeys = new HashSet<AStarTile>();
            obtainedKeys = new HashSet<KeyColor>();

            Field = new AStarTile[oldField.Field.GetLength(0), oldField.Field.GetLength(1)];
            for (int x = 0; x < Field.GetLength(0); x++)
                for (int y = 0; y < Field.GetLength(1); y++)
                    Field[x, y] = oldField.Field[x, y].Clone;

            Start = Field[newKey.Location.X, newKey.Location.Y];
            Start.Score = newKey.Score;

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

            foreach (AStarTile t in ReachedKeys)
            {
                AStarLevel newField = new AStarLevel(this, t);
                innerFields.Add(newField);
                newField.SolveLevel();
                if (newField.Finish?.Score < (Finish?.Score ?? long.MaxValue))
                {
                    Finish = newField.Finish;
                    optimalField = newField;
                }
            }
        }

        /// <summary>
        /// Keys which have been reached while solving the level.
        /// </summary>
        public HashSet<KeyColor> obtainedKeys;

        /// <summary>
        /// Inserts the optimal path into a dictionary.
        /// </summary>
        /// <param name="path"></param>
        public void FillPath(Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>> path)
        {
            if (optimalField != null)
            {
                optimalField.FillPath(path);
                path.Add(obtainedKeys, CreateOptimalPath(Start, Field[(int)optimalField?.Start.Location.X, (int)optimalField?.Start.Location.Y]));
            }
            else if (Finish != null)
                path.Add(obtainedKeys, CreateOptimalPath(Start, Field[(int)Finish?.Location.X, (int)Finish?.Location.Y]));
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
        private void TestTileInDirection(AStarTile startTile, int dx, int dy, bool specialAction = false)
        {
            if (startTile.Location.X + dx > -1 && startTile.Location.Y + dy > -1 && startTile.Location.X + dx < Field.GetLength(0) && startTile.Location.Y + dy < Field.GetLength(1))
            {
                AStarTile newTile = Field[startTile.Location.X + dx, startTile.Location.Y + dy];
                //score +1 to check if not out of range.
                if (newTile.Score > startTile.Score + 1 && newTile.TileType(obtainedKeys) != AStarTileType.blocked && startTile.Score + 1 > 0)
                    switch (newTile.TileType(obtainedKeys))
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
                                if (nextTile.TileType(null) == AStarTileType.blocked || nextTile.TileType(null) == AStarTileType.end)
                                    goto case AStarTileType.passable;
                                else
                                    TestTileInDirection(startTile, NextIncrementInSameDirection(dx), NextIncrementInSameDirection(dy));
                            break;
                        case AStarTileType.passable:
                            newTile.Score = startTile.Score + 1;
                            if(specialAction)
                                newTile.SetOldTile(startTile, 0, 0);
                            else
                                newTile.SetOldTile(startTile, dx, dy);
                            OpenTiles.Add(newTile);
                            break;
                        case AStarTileType.target:
                            newTile.Score = startTile.Score + 1;
                            ReachedKeys.Add(newTile);
                            newTile.SetOldTile(startTile, dx, dy);
                            break;
                        case AStarTileType.portal:
                            newTile.Score = startTile.Score + 1;
                            AStarTile destination = Field[newTile.Destination.X, newTile.Destination.Y];
                            if (newTile.Score + 1 < destination.Score)
                            {
                                OpenTiles.Add(destination);
                                TestTileInDirection(newTile, destination.Location.X - newTile.Location.X, destination.Location.Y - newTile.Location.Y, true);
                            }
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
            path = new Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>>();
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
            Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>> path = new Dictionary<HashSet<KeyColor>, Dictionary<Point, PlayerAction>>(HashSet<KeyColor>.CreateSetComparer());
            Level.SolveLevel();
            Level.FillPath(path);
            return path;
        }

        public void UpdateNextAction()
        {
            if(path.Count != 0)
            {
                HashSet<KeyColor> currentKeys = new HashSet<KeyColor>();
                foreach (InventoryItem item in player.DummyPlayer.Inventory.Items)
                    currentKeys.Add(item.type.ToKeyColorType());
                Dictionary<Point, PlayerAction> CurrentPath = path[currentKeys];
                if (CurrentPath.ContainsKey(player.DummyPlayer.Location))
                    player.NextAIAction = CurrentPath[player.DummyPlayer.Location];
            }
        }
    }
}
