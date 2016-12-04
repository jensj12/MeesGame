using MeesGame.Gameplay.UIObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class UIList : UIContainer
    {
        public delegate void OnItemClick(UIObject o);

        private readonly Color BACKGROUND = Color.Wheat;
        private Texture2D Background;
        
        /// <summary>
        /// Distance between the elements in the list
        /// </summary>
        private int distanceBetweenElements;
        public int DistanceBetweenElements
        {
            get { return distanceBetweenElements; }
        }

        //in retrospect, this is only usefull for the scrollbar
        protected int elementsOffset;
        public int ElementsOffset
        {
            get { return elementsOffset; }
            set { elementsOffset = value; }
        }

        protected ScrollBar scrollBar;

        public UIList(Vector2 location, Vector2 dimensions, UIContainer parent, int elementsDistance = 10) : base(location, dimensions, parent, true)
        {
            this.distanceBetweenElements = elementsDistance;
            scrollBar = new ScrollBar(this);
        }

        public override Vector2 GetChildAnchorPoint(UIObject uiObject)
        {
            int objectIndex = children.IndexOf(uiObject);

            if (objectIndex > 0)
                return new Vector2(base.GetChildAnchorPoint(this).X, distanceBetweenElements + children[objectIndex - 1].Rectangle.Bottom);

            return base.GetChildAnchorPoint(this) + new Vector2(0, -elementsOffset);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Background == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = BACKGROUND;
                Background = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                Background.SetData(colordata);
            }

            spriteBatch.Draw(Background, Rectangle, Color.White);
            base.DrawTask(gameTime, spriteBatch);
            scrollBar.Draw(gameTime, spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            scrollBar.HandleInput(inputHelper);
            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            scrollBar.Update(gameTime);
        }
    }
}
