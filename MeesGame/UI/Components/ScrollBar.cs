using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    /// <summary>
    /// Scrollbar and a background covering the vertical space the scrollbar travels over.
    /// The scrollbar is the part that can be dragged.
    /// Contrary to the other components, the scrollbar has a background because almost every scrollbar has a background
    /// </summary>
    public class ScrollBar : UIComponent
    {
        private readonly Color scrollbarColor;

        /// <summary>
        /// How many pixels the scrollbar had been lowered before beingDragged was set to true.
        /// </summary>
        private int scollBeforeDrag;

        /// <summary>
        /// Location of the mouse before beingDragged was set to true.
        /// </summary>
        private Point mouseStartDragLocation = new Point();

        /// <summary>
        /// If the mouse is currently dragging the scrollbar.
        /// </summary>
        private bool beingDragged = false;

        public ScrollBar(UIComponent parent, int width = 20, Color? backgroundColor = null, Color? scrollbarColor = null) : base(new DirectionLocation(Point.Zero, false), new InheritDimensions(inheritHeight: true, x: width))
        {
            Parent = parent;
            this.scrollbarColor = scrollbarColor ?? Utility.DrawingColorToXNAColor(DefaultUIValues.Default.ScrollbarColor);

            AddConstantComponent(new Background(Utility.SolidWhiteTexture, Utility.DrawingColorToXNAColor(DefaultUIValues.Default.ScrollbarBackgroundColor)));
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            //draws the background and the scrollbar in their respective colors with the bar on top.
            spriteBatch.Draw(Utility.SolidWhiteTexture, ScrollbarRectangleFromPoint(anchorPoint.ToPoint()), scrollbarColor);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (!Visible) return;
            //Use the input if it isn't used already and is hovering over the scrollbar.
            if (!InputUsed && AbsoluteRectangle.Contains(inputHelper.MousePosition))
            {
                Parent.InputUser = this;
                //If we press the mouse button on the scrollbar we start dragging.
                if (inputHelper.MouseLeftButtonDown() && AbsoluteScrollbarRectangle.Contains(inputHelper.MousePosition))
                {
                    mouseStartDragLocation = inputHelper.MousePosition.ToPoint();
                    scollBeforeDrag = ScrollDistance;
                    beingDragged = true;
                    PermanentInvalid = true;
                }
            }
            else if (beingDragged)
            {
                Invalidate();
                if (inputHelper.MouseLeftButtonDown())
                {
                    ScrollDistance = scollBeforeDrag + (int)(inputHelper.MousePosition.Y - mouseStartDragLocation.Y);
                }
                else
                {
                    beingDragged = false;
                    PermanentInvalid = false;
                }
            }
        }

        public override bool WantsToUseInput
        {
            get
            {
                //If we are dragging we want to keep using the input.
                return beingDragged;
            }
        }

        /// <summary>
        /// converts the ((SortedList)Parent).ChildOffset to the Distance of the scrollbar from the top.
        /// </summary>
        private int ScrollDistance
        {
            get
            {
                if (((SortedList)Parent).MaxChildOffset != 0)
                    return (int)(((SortedList)Parent).ChildOffset / (double)((SortedList)Parent).MaxChildOffset * (CachedDimensions.Y - Barheight));
                return 0;
            }
            set
            {
                if (((SortedList)Parent).MaxChildOffset != 0)
                    ((SortedList)Parent).ChildOffset = (int)(value / (double)(CachedDimensions.Y - Barheight) * ((SortedList)Parent).MaxChildOffset);
                else
                    ((SortedList)Parent).ChildOffset = 0;
            }
        }

        /// <summary>
        /// Gives the absolute location of the scrollbar
        /// </summary>
        public Rectangle AbsoluteScrollbarRectangle
        {
            get
            {
                return ScrollbarRectangleFromPoint(AbsoluteLocation.ToPoint());
            }
        }

        /// <summary>
        /// Gives the location of the scrollbar relative to its parent.
        /// </summary>
        public Rectangle RelativeScrollbarRectangle
        {
            get { return ScrollbarRectangleFromPoint(CachedRelativeLocation.ToPoint()); }
        }

        /// <summary>
        /// Returns the rectangle of the scrollbar with as location the Point given.
        /// </summary>
        /// <param name="point">Location of the rectangle.</param>
        /// <returns></returns>
        private Rectangle ScrollbarRectangleFromPoint(Point point)
        {
            return new Rectangle(point.X, point.Y + ScrollDistance, CachedDimensions.X, Barheight);
        }

        /// <summary>
        /// Gives a rectangle when drawing, is located at the origin of the screen to render the texture
        /// </summary>
        public Rectangle OriginLocationScrollbarRectangle
        {
            get { return new Rectangle(0, ScrollDistance, RelativeRectangle.Width, Barheight); }
        }

        /// <summary>
        /// Calculates the height of the scrollbar.
        /// </summary>
        private int Barheight
        {
            get
            {
                if (((SortedList)Parent).MaxChildOffset == 0)
                    return CachedDimensions.Y;
                return (int)(CachedDimensions.Y * CachedDimensions.Y / (float)((SortedList)Parent).HeightWhenExpanded);
            }
        }

        public bool BeingDragged
        {
            get { return beingDragged; }
        }
    }
}
