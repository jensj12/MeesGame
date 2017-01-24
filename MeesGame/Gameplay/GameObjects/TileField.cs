using Microsoft.Xna.Framework;
using System;

namespace MeesGame
{
    public class TileField : GameObjectGrid, IDiscreteField, ITileField
    {
        protected bool fogOfWar;

        private static readonly Point NO_POINT = new Point(-1, -1);
        private static readonly Point DEFAULT_START = new Point(1, 1);
        private Point start = NO_POINT;
        private int sightRadius = 3;

        public TileField() : base(0, 0, 0, "")
        {
        }

        public TileField(int rows, int columns, bool fogOfWar = true, int layer = 0, string id = "tiles") : base(rows, columns, layer, id)
        {
            this.fogOfWar = fogOfWar;
        }

        /// <summary>
        /// Return the type of the tile on location (x,y).
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-coordinate </param>
        /// <returns> type of the tile on location (x,y)</returns>
        public TileType GetType(int x, int y)
        {
            if (x < 0 || y < 0 || x >= base.Columns || y >= base.Rows)
            {
                return TileType.Wall;
            }
            Tile tile = Get(x, y) as Tile;
            return tile.TileType;
        }

        /// <summary>
        /// Add a tile to the tilefield on location (x,y).
        /// </summary>
        /// <param name="obj"> type of tile to add </param>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-cordinate </param>
        public void Add(Tile obj, int x, int y)
        {
            if (OutOfTileField(x, y))
                return;

            if (OnCornerOfTileField(x, y))
                // On the corners only Wall tiles are allowed
                if (obj.TileType != TileType.Wall)
                    return;

            if (OnEdgeOfTileField(x, y))
            {
                // If You are on the edge, but not on a corner, you can place and EndTile or a wallTile
                if (obj.TileType != TileType.Wall && obj.TileType != TileType.End)
                    return;
            }
            else
            {
                // If you are not on the edge, you can't place and EndTile
                if (obj.TileType == TileType.End)
                    return;
            }

            base.Add(obj, x, y);
            obj.Location = new Point(x, y);
        }

        /// <summary>
        /// Resizes the grid to the given values. Meant for use in editor only.
        /// </summary>
        /// <param name="rows">Number of rows in the resized grid</param>
        /// <param name="columns">Number of columns in the resized grid</param>
        public void Resize(int rows, int columns)
        {
            GameObject[,] oldGrid = grid;
            grid = new GameObject[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (y < oldGrid.GetLength(1) && x < oldGrid.GetLength(0))
                        grid[x, y] = oldGrid[x, y];
                    else
                        Add(new FloorTile(), x, y);
                }
            }
        }

        /// <summary>
        /// Return the tile on the given location.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-coordinate </param>
        /// <returns> Tile on location (x,y) </returns>
        public Tile GetTile(int x, int y)
        {
            if (OutOfTileField(x, y))
                return new WallTile();
            return (Tile)Get(x, y);
        }

        #region tile position methods

        /// <summary>
        /// Checks if the x and y are outside the bounds of the tilefield.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-coordinate </param>
        /// <returns> Whether a tile is out of the tilefield </returns>
        public bool OutOfTileField(int x, int y)
        {
            return x < 0 || y < 0 || x >= base.Columns || y >= base.Rows;
        }

        /// <summary>
        /// Checks if a location is outside the bounds of the tilefield.
        /// </summary>
        /// <param name="location"> Location </param>
        /// <returns> Whether a tile is out of the tilefield </returns>
        public bool OutOfTileField(Point location)
        {
            return OutOfTileField(location.X, location.Y);
        }

        /// <summary>
        /// Checks if the location is on the edge of the TileField.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-coordinate </param>
        /// <returns> Whether a tile is on the edge of the tilefield. </returns>
        public bool OnEdgeOfTileField(int x, int y)
        {
            return (x == 0 || y == 0 || x == base.Columns - 1 || y == base.Rows - 1);
        }

        /// <summary>
        /// Checks if the location is on the corner of the TileField.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-coordinate </param>
        /// <returns> Whether a tile is on a corner of the tilefield. </returns>
        public bool OnCornerOfTileField(int x, int y)
        {
            return ((x == 0 || x == base.Columns - 1) && (y == 0 || y == base.Rows - 1));
        }

        /// <summary>
        /// Checks if the location is near the edge of the TileField.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="y"> y-coordinate </param>
        /// <returns> This bool returns whether a tile is next to the edge of the tilefield. </returns>
        public bool NearEdgeOfTileField(int x, int y)
        {
            return (x == 1 || y == 1 || x == base.Columns - 2 || y == base.Rows - 2);
        }

        #endregion

        public Tile GetTile(Point location)
        {
            return GetTile(location.X, location.Y);
        }

        /// <summary>
        /// Update the graphics of all tiles to match the surroundings.
        /// </summary>
        public void UpdateGraphicsToMatchSurroundings()
        {
            foreach (Tile tile in Objects)
            {
                tile.UpdateGraphicsToMatchSurroundings();
            }
        }

        /// <summary>
        /// Update the graphics of the tiles around the given location to match the surroundings.
        /// </summary>
        public void UpdateGraphicsToMatchSurroundings(Point location)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (OutOfTileField(location.X + x, location.Y + y)) continue;
                    (Objects[location.X + x, location.Y + y] as Tile).UpdateGraphicsToMatchSurroundings();
                }
            }
        }

        /// <summary>
        /// Update the graphics of the tiles.
        /// </summary>
        public void UpdateGraphics()
        {
            foreach (Tile tile in Objects)
            {
                tile.UpdateGraphics();
            }
        }

        public void UpdatePortals()
        {
            foreach (Tile tile in Objects)
            {
                tile.UpdatePortals();
            }
        }

        #region reveal area methods

        /// <summary>
        /// Reveals an area of tiles.
        /// </summary>
        /// <param name="point"> Center point of the area </param>
        public void RevealArea(Point point)
        {
            revealAroundTile(point);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                revealPath(point, direction.ToPoint(), sightRadius, point, direction);
            }
        }

        /// <summary>
        /// Reveals a tile and part of the area around it.
        /// </summary>
        /// <param name="point"> The tile to be revealed </param>
        private void revealAroundTile(Point point)
        {
            for (int i = point.X - 1; i <= point.X + 1; i++)
            {
                for (int j = point.Y - 1; j <= point.Y + 1; j++)
                {
                    if (!OutOfTileField(i, j))
                    {
                        //using bitwise or-operator in the setter
                        if (i <= point.X && j >= point.Y) { GetTile(i, j).Revealed = 1; }
                        if (i <= point.X && j <= point.Y) { GetTile(i, j).Revealed = 2; }
                        if (i >= point.X && j <= point.Y) { GetTile(i, j).Revealed = 4; }
                        if (i >= point.X && j >= point.Y) { GetTile(i, j).Revealed = 8; }
                    }
                }
            }
        }

        /// <summary>
        /// Recursively reveals the path until the limit has been reached or vision is obstructed.
        /// </summary>
        /// <param name="point"> Current point </param>
        /// <param name="direction"> Current direction </param>
        /// <param name="limit"> Limit, how far you can see from here</param>
        /// <param name="startPoint"> Starting Point </param>
        /// <param name="startDirection"> Starting Direction </param>
        private void revealPath(Point point, Point direction, int limit, Point startPoint, Direction startDirection)
        {
            //check if you're still on the map, while that is the case, vision is not obstructed, 
            //and the distance limit has not been reached reveal the area around the tile.
            if (!OutOfTileField((point.X += direction.X), (point.Y += direction.Y))
                && !GetTile(point.X, point.Y).ObstructsVision
                && limit > 0)
            {
                // Don't reveal the corners beacuse it looks better.
                if ((point.X - startPoint.X != sightRadius && point.X - startPoint.X != -sightRadius) ||
                   (point.Y - startPoint.Y != sightRadius && point.Y - startPoint.Y != -sightRadius))
                {
                    revealAroundTile(point);
                }

                // All of the directions branch off, for example if the startDirection is North, 
                // you'll want to continue in the directions North-West, North and North-East.
                for (int i = -1; i <= 1; i++)
                {
                    Point newDirection = direction;
                    if (startDirection.ToPoint().X == 0)
                        newDirection.X += i;
                    else
                        newDirection.Y += i;
                    revealPath(point, newDirection, limit - 1, startPoint, startDirection);
                }
            }
        }

        public bool UpdateStart()
        {
            start = DEFAULT_START;
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (GetTile(x, y).TileType == TileType.Start)
                    {
                        start = new Point(x, y);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Should be called when the player enters a Tile so that the graphics can be adjusted.
        /// </summary>
        /// <param name="location">The location of the Tile the player visits</param>
        public void Visit(Point location)
        {
            RevealArea(location);
            GetTile(location).MarkVisited();
        }

        #endregion

        #region properties

        public Point Start
        {
            get
            {
                if (start == NO_POINT)
                    UpdateStart();
                return start;
            }
        }

        public bool FogOfWar
        {
            get
            {
                return fogOfWar;
            }
            set
            {
                fogOfWar = value;
            }
        }

        public Vector2 CellDimensions
        {
            get
            {
                return new Vector2(CellWidth, CellHeight);
            }
        }

        #endregion
    }
}
