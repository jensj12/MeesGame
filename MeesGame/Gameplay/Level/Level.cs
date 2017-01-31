using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MeesGame
{
    public class Level : GameObjectList
    {
        public const int DEFAULT_NUM_ROWS = 31;
        public const int DEFAULT_NUM_COLS = 31;
        protected int numRows = DEFAULT_NUM_ROWS;
        protected int numColumns = DEFAULT_NUM_COLS;
        protected const int CELL_HEIGHT = 64;
        protected const int CELL_WIDTH = 64;
        protected readonly TimeSpan timeBetweenActions = TimeSpan.FromMilliseconds(210);
        protected Point start;
        protected AnimatedMovingGameObject player;
        private TileField tiles;

        public Level(bool fogOfWar = true)
        {
            Tiles = new TileField(numRows, numColumns, fogOfWar, 0, "tiles");
        }

        public TileField Tiles
        {
            get
            {
                return tiles;
            }
            protected set
            {
                tiles = value;
                tiles.CellHeight = CELL_HEIGHT;
                tiles.CellWidth = CELL_WIDTH;
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
            Camera camera = Camera;
            camera.UpdateCamera();
            base.Draw(gameTime, spriteBatch);
            camera.ResetCamera();
        }

        protected virtual void usePlayer(AnimatedMovingGameObject player)
        {
            this.player = player;

            Camera camera = new Camera(null, player);
            camera.Add(this.Tiles);
            camera.Add(this.player);
            Add(camera);
        }

        public Camera Camera
        {
            get { return Find("camera") as Camera; }
        }
    }
}
