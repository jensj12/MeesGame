using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

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

        public Level(bool fogOfWar = true)
        {
            tiles = new TileField(numRows, numColumns, fogOfWar, 0, "tiles");
            tiles.CellHeight = CELL_HEIGHT;
            tiles.CellWidth = CELL_WIDTH;
        }

        public Player Player
        {
            get { return player; }
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

        protected void usePlayer(Player player, int screenWidth = -1, int screenHeight = -1)
        {
            this.player = player;

            if (screenWidth == -1) screenWidth = GameEnvironment.Screen.X;
            if (screenHeight == -1) screenHeight = GameEnvironment.Screen.Y;

            Camera camera = new Camera(new Point(screenWidth, screenHeight), player);
            Add(camera);
            camera.Add(this.tiles);
            camera.Add(this.player);
        }
    }
}