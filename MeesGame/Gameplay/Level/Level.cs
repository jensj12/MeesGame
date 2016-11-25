using Microsoft.Xna.Framework;

namespace MeesGame
{
    class Level : GameObjectList
    {
        protected const int NUM_ROWS = 12;
        protected const int NUM_COLUMNS = 22;
        protected const int CELL_HEIGHT = 64;
        protected const int CELL_WIDTH = 64;
        protected Point start;
        protected Player player;
        protected TileFieldView tiles;
        public Level(int levelindex = 0)
        {
            start = new Point(0, 0);
            
            TileField tileField = new TileField(NUM_ROWS, NUM_COLUMNS, 0, "tiles");
            tileField.CellHeight = CELL_HEIGHT;
            tileField.CellWidth = CELL_WIDTH;

            //Temporary initialisation of empty tiles
            for (int x = 0; x < NUM_COLUMNS; x++)
            {
                for (int y = 0; y < NUM_ROWS; y++)
                {
                    tileField.Add(new Tile("floor"), x, y);
                }
            }

            this.player = new Player(this,start);
            this.tiles = new TileFieldView(player, tileField);
            Add(this.tiles);
            Add(this.player);
        }

        public TileFieldView Tiles
        {
            get
            {
                return tiles;
            }
        }
    }
}
