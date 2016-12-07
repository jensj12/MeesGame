using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    public class UIList : UIContainer
    {
        public delegate void OnItemClick(UIObject o);
        public event OnItemClick onItemClick;

        private readonly Color BACKGROUND = Color.Wheat;
        private Texture2D Background;

        //distance each of the following elements has
        private int elementsDistance;
        public int ElementsDistance
        {
            get { return elementsDistance;}
        }

        //in retrospect, this is only usefull for the scrollbar
        protected int elementsOffset;
        public int ElementsOffset
        {
            get { return elementsOffset; }
            set { elementsOffset = value; }
        }       

        protected ScrollBar scrollBar;

        public UIList(Vector2 location, Vector2 dimensions, UIContainer parent, int elementsDistance = 10) : base(location, dimensions, parent)
        {
            this.elementsDistance = elementsDistance;
            scrollBar = new ScrollBar(this);
        }

        public override Vector2 GetChildAnchorPoint(UIObject uiObject)
        {
            int objectIndex = children.IndexOf(uiObject);

            if(objectIndex > 0)
                return new Vector2(0, elementsDistance + children[objectIndex - 1].RelativeRectangle.Bottom);

            //if we only scale the first element, the rest will follow.
            return new Vector2(0, -elementsOffset);
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

            spriteBatch.Draw(Background, OriginLocationRectangle, Color.White);
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
