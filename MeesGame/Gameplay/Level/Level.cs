using Microsoft.Xna.Framework;
using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class Level : GameObjectList, IXmlSerializable
    {
        protected int numRows = 31;
        protected int numColumns = 31;
        protected const int CELL_HEIGHT = 64;
        protected const int CELL_WIDTH = 64;
        protected TimeSpan timeBetweenActions = TimeSpan.FromMilliseconds(300);
        protected Point start;
        protected Player player;
        protected TileField tiles;
        public Level(int levelindex = 0)
        {
            tiles = new TileField(numRows, numColumns, 0, "tiles");
            tiles.CellHeight = CELL_HEIGHT;
            tiles.CellWidth = CELL_WIDTH;

            start = new Point(GameEnvironment.Random.Next(tiles.Columns / 2) * 2, GameEnvironment.Random.Next(tiles.Rows / 2) * 2);
            tiles = MeesGen.MazeGenerator.GenerateMaze(tiles, start.X, start.Y);
            tiles.UpdateGraphicsToMatchSurroundings();

            this.player = new HumanPlayer(this, tiles, start);
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
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
