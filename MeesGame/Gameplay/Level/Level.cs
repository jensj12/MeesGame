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
        protected TimeSpan timeBetweenActions = TimeSpan.FromMilliseconds(300);
        protected Point start;
        protected Player player;
        protected TileField tiles;
        public Level(int levelindex = 0)
        {
            start = new Point(1, 1);
            
            tiles = new TileField(numRows, numColumns, 0, "tiles");
            tiles.CellHeight = CELL_HEIGHT;
            tiles.CellWidth = CELL_WIDTH;

            //Temporary initialisation of empty tiles
            for (int x = 0; x < numColumns; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    tiles.Add(new FloorTile(), x, y);
                }
            }

            this.player = new HumanPlayer(this,tiles,start);
            Camera camera = new Camera(player);
            Add(camera);
            camera.Add(this.tiles);
            camera.Add(this.player);
        }

        public TileField Tiles
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Camera camera = Find("camera") as Camera;
            camera.UpdateCamera();
            base.Draw(gameTime, spriteBatch);
            camera.ResetCamera();
        }
    }
}
