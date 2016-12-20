using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class UIList : UIContainer
    {
        protected ScrollBar scrollBar;

        /// <summary>
        /// Distance from the parent at which the children should be drawn.
        /// </summary>
        protected int objectsOffset;

        /// <summary>
        /// Distance between the child objects
        /// </summary>
        private int objectsDistance;

        /// <summary>
        /// A sorted list in which children are drawn underneath each other.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="objectDistance">Distance between the objects in the list</param>
        /// <param name="backgroundColor"></param>
        public UIList(Vector2? location = null, Vector2? dimensions = null, int objectDistance = 10, Color? backgroundColor = null) : base(location, dimensions, backgroundColor)
        {
            this.objectsDistance = objectDistance;
            scrollBar = new ScrollBar(this);
        }

        /// <summary>
        /// Returns an offset from the location of this object where the child should be drawn
        /// For example, when scrolling down we want every object in the list to be drawn at a greater Y value
        /// </summary>
        /// <param name="uiObject"></param>
        /// <returns></returns>
        public override Vector2 GetChildAnchorPoint(UIObject uiObject)
        {
            int objectIndex = children.IndexOf(uiObject);

            if (objectIndex > 0)
                return new Vector2(0, objectsDistance + children[objectIndex - 1].RelativeRectangle.Bottom);

            //if we only scale the first element, the rest will follow.
            return new Vector2(0, -objectsOffset);
        }

        /// <summary>
        /// Adds a child to the sorted list
        /// </summary>
        /// <param name="child"></param>
        public override void AddChild(UIObject child)
        {
            base.AddChild(child);

            scrollBar.UpdateParentHeightWhenShowingAllChildren();
        }

        public override void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            scrollBar.RenderTexture(gameTime, spriteBatch);
            base.RenderTexture(gameTime, spriteBatch);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawTask(gameTime, spriteBatch);
            scrollBar.Draw(gameTime, spriteBatch);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //Because the scrollbar is drawn first it needs to be the first object to have its input checked
            scrollBar.HandleInput(inputHelper);
            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            //when scrolling down the scrollbar should be updated first
            scrollBar.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
            children.Reset();
            scrollBar.UpdateParentHeightWhenShowingAllChildren();
        }

        public int ElementsDistance
        {
            get { return objectsDistance; }
        }

        public int ElementsOffset
        {
            get { return objectsOffset; }
            set { objectsOffset = value; }
        }
    }
}