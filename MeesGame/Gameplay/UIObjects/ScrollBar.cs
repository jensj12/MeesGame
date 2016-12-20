using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class ScrollBar : UIObject
    {
        ///Variables defining the colors of the scrollbar
        private readonly Color scrollbarColor = Color.Aqua;

        ///Stores the total length of elements in the container.
        private int parentHeightWhenShowingAllChildren;

        ///The scroll distance before we began dragging. This variable is needed because otherwise we'd have
        ///To use the less accurate change of coordinates and apply that to the current scrollbar location
        private int scolldistanceStartDrag;

        ///Rhe mouse position when we start dragging the scrollbar
        private Point mouseStartDragLocation = new Point();

        ///whether the scrollbar is being dragged
        private bool beingDragged = false;

        public ScrollBar(UIList parent, int width = 20, Color? backgroundColor = null, Color? scrollbarColor = null) : base(new Vector2(parent.Dimensions.X - width, 0), new Vector2(width, parent.Dimensions.Y), backgroundColor ?? Color.Beige)
        {
            this.Parent = parent;
            this.scrollbarColor = scrollbarColor ?? Color.Aqua;
            UpdateParentHeightWhenShowingAllChildren();
        }

        /// <summary>
        /// Updates the size the parent would be if it were fully expanded. Always smaller than
        /// its size as defined in its width and height
        /// </summary>
        public void UpdateParentHeightWhenShowingAllChildren()
        {
            if (Parent.Children.Count > 0)
                parentHeightWhenShowingAllChildren = Parent.Children[Parent.Children.Count - 1].AbsoluteRectangle.Bottom - Parent.Children[0].AbsoluteRectangle.Top;
            if (parentHeightWhenShowingAllChildren <= AbsoluteRectangle.Height)
            {
                parentHeightWhenShowingAllChildren = Parent.AbsoluteRectangle.Height;
                //automatically remove scrollbar when it is not necessary
                Visible = false;
            }
            else
                Visible = true;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawTask(gameTime, spriteBatch);
            //draws the scrollbar and the bar in their respective colors with the bar on top
            spriteBatch.Draw(SolidWhiteTexture, OriginLocationBarRectangle, scrollbarColor);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (!Visible) return;
            //Use the input if it isn't used already and is hovering over the scrollbar
            if (!Parent.InputEaten && AbsoluteRectangle.Contains(inputHelper.MousePosition))
            {
                Parent.InputEater = this;
                //If we press the mouse button on the bar we start dragging
                if (inputHelper.MouseLeftButtonDown() && AbsoluteBarRectangle.Contains(inputHelper.MousePosition))
                {
                    mouseStartDragLocation = inputHelper.MousePosition.ToPoint();
                    scolldistanceStartDrag = ScrollDistance;
                    beingDragged = true;
                }
            }
            else if (beingDragged)
            {
                Invalidate();
                if (inputHelper.MouseLeftButtonDown())
                {
                    //The scroll distance is the scroll distance we started with plus the distance the mouse has
                    //moved since we started dragging
                    ScrollDistance = scolldistanceStartDrag + (int)(inputHelper.MousePosition.Y - mouseStartDragLocation.Y);
                    if (AbsoluteBarRectangle.Bottom > AbsoluteRectangle.Bottom)
                        ScrollDistance = AbsoluteRectangle.Height - AbsoluteBarRectangle.Height;
                    if (ScrollDistance < 0)
                        ScrollDistance = 0;
                }
                else
                {
                    beingDragged = false;
                }
            }
        }

        /// <summary>
        /// If we are dragging we want to keep using the input
        /// </summary>
        public override bool WantsToEatInput
        {
            get
            {
                return beingDragged;
            }
        }

        /// <summary>
        /// We need to override our relative location because we are not part of the list, so we never need the
        /// anchor point, only to the position of its parent
        /// </summary>
        public override Vector2 RelativeLocation
        {
            get
            {
                return new Vector2(Parent.Dimensions.X - Dimensions.X, 0);
            }
        }

        ///How big the offset it from the scrollbar to the top of the parent element is. gets and set it in the parent.
        private int ScrollDistance
        {
            get
            {
                if (parentHeightWhenShowingAllChildren != AbsoluteRectangle.Height)
                    //A simple way to picture this calculation is to think that the maximum distance of the elements offset is equal to totalElementsSize - Rectangle.Height
                    //and the maximum distance the scrollbar can travel is Rectangle.Height - Barheight, so it will always remain between those bounds and linearly scale
                    //offset distance
                    return ((UIList)Parent).ObjectsOffset * (AbsoluteRectangle.Height - Barheight) / (parentHeightWhenShowingAllChildren - AbsoluteRectangle.Height);
                return 0;
            }
            set
            {
                if (parentHeightWhenShowingAllChildren - Dimensions.Y != 0)
                    //The ratio between the maximum distance of the elementsoffset * the maximum distance the scrollbar can travel, and multiply this by a number between zero
                    //and the maximum distance the scrollbar can travel
                    ((UIList)Parent).ObjectsOffset = (int)(value * (parentHeightWhenShowingAllChildren - Dimensions.Y) / (double)(Dimensions.Y - Barheight));
                else
                    ((UIList)Parent).ObjectsOffset = 0;
            }
        }

        /// <summary>
        /// Gives the absolute location of the bar
        /// </summary>
        public Rectangle AbsoluteBarRectangle
        {
            get
            {
                if (parentHeightWhenShowingAllChildren <= Dimensions.Y)
                    return AbsoluteRectangle;
                else
                {
                    return new Rectangle(AbsoluteRectangle.X, AbsoluteRectangle.Y + ScrollDistance, AbsoluteRectangle.Width, Barheight);
                }
            }
        }

        /// <summary>
        /// Gives the location of the bar relative to the scrollbar's parent
        /// </summary>
        public Rectangle RelativeBarRectangle
        {
            get { return new Rectangle((int)RelativeLocation.X, ScrollDistance, RelativeRectangle.Width, Barheight); }
        }

        /// <summary>
        /// Gives a rectangle when drawing, is located at the origin of the screen to render the texture
        /// </summary>
        public Rectangle OriginLocationBarRectangle
        {
            get { return new Rectangle(0, ScrollDistance, RelativeRectangle.Width, Barheight); }
        }

        /// <summary>
        /// Calculates the height of the bar
        /// </summary>
        private int Barheight
        {
            get
            {
                if (parentHeightWhenShowingAllChildren == 0)
                    return (int)Dimensions.Y;
                return (int)(Dimensions.Y / (double)parentHeightWhenShowingAllChildren * Dimensions.Y);
            }
        }

        public bool BeingDragged
        {
            get { return beingDragged; }
        }
    }
}