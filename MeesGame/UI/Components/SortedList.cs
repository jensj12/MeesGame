using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class SortedList : UIComponent
    {
        /// <summary>
        /// Distance between the child objects
        /// </summary>
        public int distanceBetweenChildren;

        protected ScrollBar scrollBar;

        /// <summary>
        /// cached height when expanded
        /// </summary>
        private int heightWhenExpanded;

        /// <summary>
        /// Distance from the parent at which the children should be drawn.
        /// </summary>
        public int childOffset;

        /// <summary>
        /// A sorted list in which children are drawn underneath each other.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="objectDistance">Distance between the objects in the list</param>
        /// <param name="backgroundColor"></param>
        public SortedList(Location location = null, Dimensions dimensions = null, int DistanceBetweenChildren = 10, Color? backgroundColor = null) : base(location, dimensions)
        {
            this.distanceBetweenChildren = DistanceBetweenChildren;
            AddConstantComponent(new Background(Utility.SolidWhiteTexture, backgroundColor ?? Utility.DrawingColorToXNAColor(DefaultUIValues.Default.FileExplorerBackground)));
            scrollBar = new ScrollBar(this);
            scrollBar.Visible = false;
        }

        /// <summary>
        /// Returns an offset from the location of this object where the child should be drawn
        /// For example, when scrolling down we want every object in the list to be drawn at a greater Y value
        /// </summary>
        /// <param name="uiObject"></param>
        /// <returns></returns>
        public override Vector2 ChildAnchorPoint(UIComponent uiObject)
        {
            int objectIndex = children.IndexOf(uiObject);

            if (objectIndex < 0) return Vector2.Zero;

            if (objectIndex > 0)
                return new Vector2(0, distanceBetweenChildren + children[objectIndex - 1].RelativeRectangle.Bottom);

            //if we only scale the first element, the rest will follow.
            return new Vector2(0, -childOffset);
        }

        /// <summary>
        /// Adds a child to the sorted list
        /// </summary>
        /// <param name="child"></param>
        public override void AddChild(UIComponent child)
        {
            base.AddChild(child);

            UpdateHeightWhenExpanded();
        }

        public override void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            scrollBar.RenderTexture(gameTime, spriteBatch);
            base.RenderTexture(gameTime, spriteBatch);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            scrollBar.Draw(gameTime, spriteBatch, anchorPoint);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //Because the scrollbar is drawn first it needs to be the first object to have its input checked
            scrollBar.HandleInput(inputHelper);

            if (AbsoluteRectangle.Contains(inputHelper.MousePosition))
            {
                ChildOffset = ChildOffset - inputHelper.ScrollDelta;
            }

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
            scrollBar.Reset();
        }

        public override bool PermanentInvalid
        {
            get
            {
                return base.PermanentInvalid;
            }

            set
            {
                if (scrollBar?.PermanentInvalid ?? false)
                {
                    base.PermanentInvalid = true;
                    return;
                }
                base.PermanentInvalid = value;
            }
        }

        /// <summary>
        /// Distance from the parent at which the children should be drawn.
        /// </summary>
        public int ChildOffset
        {
            get { return childOffset; }
            set
            {
                if (value < 0)
                    childOffset = 0;
                else if (value < MaxChildOffset)
                    childOffset = value;
                else
                    childOffset = MaxChildOffset;
                scrollBar?.Invalidate();
            }
        }

        /// <summary>
        /// Updates the height the parent would have if it were fully expanded.
        /// </summary>
        public void UpdateHeightWhenExpanded()
        {
            if (children.Count > 0)
            {
                int childrenHeight = (int)(Children[Children.Count - 1].AbsoluteRectangle.Bottom - AbsoluteLocation.Y);
                if (childrenHeight > CurrentDimensions.Y)
                {
                    heightWhenExpanded = childrenHeight;
                    scrollBar.Visible = true;
                }
                else
                {
                    heightWhenExpanded = CurrentDimensions.Y;
                    scrollBar.Visible = false;
                }
            }
        }

        /// <summary>
        /// Height when every child would be fully visible.
        /// </summary>
        public int HeightWhenExpanded
        {
            get
            {
                return heightWhenExpanded;
            }
        }

        /// <summary>
        /// Gives the maximal child offset. Any greater child offset would result in no more children being shown.
        /// </summary>
        public int MaxChildOffset
        {
            get { return HeightWhenExpanded - CurrentDimensions.Y; }
        }

        public int DistanceBetweenChildren
        {
            get { return distanceBetweenChildren; }
            set { distanceBetweenChildren = value; }
        }
    }
}
