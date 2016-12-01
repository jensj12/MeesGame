using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIList : UIObject
    {
        private readonly Color BACKGROUND = Color.Wheat;
        private Texture2D Background;

        private int elementsDistance;
        public int ElementsDistance
        {
            get { return elementsDistance;}
        }
        protected int elementsOffset;
        public int ElementsOffset
        {
            get { return elementsOffset; }
            set { elementsOffset = value; }
        }
        protected ScrollBar scrollBar;

        public UIList(Vector2 location, Vector2 dimensions, UIObject parent, int elementsDistance = 10) : base(location, dimensions, parent, true)
        {
            scrollBar = new ScrollBar(this);
        }

        public void MoveDistanceDown(int distance)
        {
            elementsOffset = distance;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            scrollBar.Draw(gameTime, spriteBatch);
        }

        public override void DrawSelf(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Background == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = BACKGROUND;
                Background = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                Background.SetData(colordata);
            }

            spriteBatch.Draw(Background, Rectangle, Color.White);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (Rectangle.Contains(inputHelper.MousePosition))
            {
                if (scrollBar.BarRectangle.Contains(inputHelper.MousePosition) || scrollBar.BeingDragged)
                {
                    scrollBar.HandleInput(inputHelper);
                }
                else
                    children.HandleInput(inputHelper);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            scrollBar.Update(gameTime);
        }

    }
}
