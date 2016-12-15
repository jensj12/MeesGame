using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class UIList : UIContainer
    {
        public event ClickEventHandler onItemClick;

        protected ScrollBar scrollBar;

        public UIList(Vector2? location = null, Vector2? dimensions = null, int elementsDistance = 10, Color? backgroundColor = null) : base(location, dimensions, backgroundColor)
        {
            this.elementsDistance = elementsDistance;
            scrollBar = new ScrollBar(this);
        }

        public override Vector2 GetChildAnchorPoint(UIObject UIObject)
        {
            int objectIndex = children.IndexOf(UIObject);

            if(objectIndex > 0)
                return new Vector2(0, elementsDistance + children[objectIndex - 1].RelativeRectangle.Bottom);

            //if we only scale the first element, the rest will follow.
            return new Vector2(0, -elementsOffset);
        }

        public override void AddChild(UIObject child)
        {
            base.AddChild(child);

            scrollBar.UpdateParentHeightWhenShowingAllChildren();
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
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

        //distance each of the following elements has
        private int elementsDistance;
        public int ElementsDistance
        {
            get { return elementsDistance; }
        }

        //in retrospect, this is only usefull for the scrollbar
        protected int elementsOffset;
        public int ElementsOffset
        {
            get { return elementsOffset; }
            set { elementsOffset = value; }
        }
    }
}
