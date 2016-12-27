using Microsoft.Xna.Framework;

namespace MeesGame
{
    public class TileField : GameObjectGrid, IDiscreteField, ITileField
    {
        protected bool fogOfWar;

        private static readonly Point NO_POINT = new Point(-1, -1);
        private static readonly Point DEFAULT_START = Point.Zero;
        private Point start = NO_POINT;
        public Point Start
        {
            get
            {
                if (start == NO_POINT)
                    UpdateStart();
                return start;
            }
        }

        public TileField() : base(0, 0, 0, "")
        {
        }

        public TileField(int rows, int columns, bool fogOfWar = true, int layer = 0, string id = "tiles") : base(rows, columns, layer, id)
        {
            this.fogOfWar = fogOfWar;
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

        public TileType GetType(int x, int y)
        {
            if (x < 0 || y < 0 || x >= base.Columns || y >= base.Rows)
            {
                return TileType.Wall;
            }
            Tile tile = Get(x, y) as Tile;
            return tile.TileType;
        }

        public void Add(Tile obj, int x, int y)
        {
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

        public Tile GetTile(int x, int y)
        {
            if (OutOfTileField(x, y))
                return new WallTile();
            return (Tile)Get(x, y);
        }

        ///this bool returns if the x and y are outside the bounds of the tilefield
        public bool OutOfTileField(int x, int y)
        {
            if (x < 0 || y < 0 || x >= base.Columns || y >= base.Rows)
            {
                return true;
            }
            return false;
        }

        public Tile GetTile(Point location)
        {
            return GetTile(location.X, location.Y);
        }

        public void UpdateGraphicsToMatchSurroundings()
        {
            foreach (Tile tile in Objects)
            {
                tile.UpdateGraphicsToMatchSurroundings();
            }
        }

        public void UpdateGraphics()
        {
            foreach (Tile tile in Objects)
            {
                tile.UpdateGraphics();
            }
        }

        public void RevealArea(Point a)
        {
            for (int i = a.X - 1; i <= a.X + 1; i++)
            {
                for (int j = a.Y - 1; j <= a.Y + 1; j++)
                {
                    if (i >= 0 && i < this.Columns && j >= 0 && j < this.Rows) ((Tile)this.grid[i, j]).Revealed = true;
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

        public Vector2 CellDimensions
        {
            get
            {
                return new Vector2(CellWidth, CellHeight);
            }
        }
    }
}
