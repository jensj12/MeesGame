using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class Level : GameObjectList
    {
        protected int numRows = 30;
        protected int numColumns = 30;
        protected const int CELL_HEIGHT = 64;
        protected const int CELL_WIDTH = 64;
        protected TimeSpan timeBetweenActions = TimeSpan.FromMilliseconds(200);
        protected Point start;
        protected Player player;
        protected TileFieldView tiles;

        public Level(Point fieldSize, int levelindex = 0)
        {
            start = new Point(1, 1);
            
            TileField tileField = new TileField(numRows, numColumns, 0, "tiles");
            tileField.CellHeight = CELL_HEIGHT;
            tileField.CellWidth = CELL_WIDTH;

            //Temporary initialisation of empty tiles
            for (int x = 0; x < numColumns; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    tileField.Add(Tile.GetTileFromTileType(TileType.Floor), x, y);
                }
            }

            this.player = new Player(this,tileField,start);
            this.tiles = new TileFieldView(player, tileField);
            Camera camera = new Camera(fieldSize, player);
            Add(camera);
            camera.Add(this.tiles);
            camera.Add(this.player);
        }

        public TileFieldView Tiles
        {
            get
            {
                return tiles;
            }
        }

        public TimeSpan TimeBetweenActions
        {
            get
            {
                return timeBetweenActions;
            }
        }

        public Player Player
        {
            get { return player; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Camera camera = Find("camera") as Camera;
            camera.UpdateCamera();
            base.Draw(gameTime, spriteBatch);
        }
    }
}
