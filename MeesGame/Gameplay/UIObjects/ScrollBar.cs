using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class ScrollBar : UIObject
    {
        /// <summary>
        /// the word Bar in this class is the inner part of the scrollbar which moves when being dragged.
        /// </summary>

        //variables defining the colors of the scrollbar
        private readonly Color barColor = Color.Aqua;
        private readonly Color backColor = Color.Beige;

        //stores the total length of elements in the container.
        private int totalElementsSize;
        //the scroll distance before we began draggin. This variable is needed because otherwise we'd have
        //to use the less acurate change of coordinates and apply that to the current scrollbarlocation
        private int scolldistanceStartDrag;

        //a solid color is inserted into this texture for painting a solid color
        private Texture2D solidColorTexture;

        //the mouse position when we start dragging the scrollbar
        private Point mouseStartDragLocation = new Point();

        //whether the scrollbar is being dragged
        private bool beingDragged = false;

        /// <summary>
        /// we need to override our relative location because we are not part of the list, so we don't need the
        /// anchor point, only to the position of its parent
        /// </summary>
        public override Vector2 RelativeLocation
        {
            get
            {
                return new Vector2(parent.Dimensions.X - Dimensions.X, 0);
            }
        }

        ///how big the offset it from the scrollbar to the top of the parent element is. gets and set it in the parent.
        private int ScrollDistance
        {
            get
            {
                if (totalElementsSize != AbsoluteRectangle.Height)
                    ///a simple way to picture this calculation is to think that the maximum distance of the elementsoffset is equal to totalElementsSize - Rectangle.Height 
                    ///and the maximum distance the scrollbar can travel is Rectangle.Height - Barheight, so it will always remain between those bounds and linearly scale
                    ///offset distance
                    return ((UIList)parent).ElementsOffset * (AbsoluteRectangle.Height - Barheight) / (totalElementsSize - AbsoluteRectangle.Height);
                return 0;
            }
            set
            {
                if (totalElementsSize - Dimensions.Y != 0)
                    ///the ratio between the maximum distance of the elementsoffset * the maximum distance the scrollbar can travel, and multiply this by a number between zero
                    ///and the maximum distance the scrollbar can travel
                    ///
                    ((UIList)parent).ElementsOffset = (int)(value * (totalElementsSize - Dimensions.Y) / (double)(Dimensions.Y - Barheight));
                else
                    ((UIList)parent).ElementsOffset = 0;
            }
        }

        /// <summary>
        /// gives the absolute location of the bar
        /// </summary>
        public Rectangle AbsoluteBarRectangle
        {
            get
            {
                if (totalElementsSize <= Dimensions.Y)
                    return AbsoluteRectangle;
                else
                {
                    return new Rectangle(AbsoluteRectangle.X, AbsoluteRectangle.Y + ScrollDistance, AbsoluteRectangle.Width, Barheight);
                }
            }
        }

        /// <summary>
        /// gives the location relative to the parent of the barr
        /// </summary>
        public Rectangle RelativeBarRectangle
        {
            get { return new Rectangle((int)RelativeLocation.X, ScrollDistance, RelativeRectangle.Width, Barheight); }
        }

        /// <summary>
        /// gives a rectangle when drawing, is located at the origin of the screen to render the texture
        /// </summary>
        public Rectangle OriginLocationBarRectangle
        {
            get { return new Rectangle(0, ScrollDistance, RelativeRectangle.Width, Barheight); }
        }

        /// <summary>
        /// calculates the height of the bar
        /// </summary>
        private int Barheight
        {
            //basically it takes the ratio between the height of the scrollbar and the total size, and
            //multiplies this ratio by the total height of the scrollbar. This results in the bar
            //always being smaller than the scrollbar. totalelementssize is always at least as big as
            //the parent container
            get { return (int)(Dimensions.Y / (double)totalElementsSize * Dimensions.Y); }
        }

        public bool BeingDragged
        {
            get { return beingDragged; }
        }

        public ScrollBar(UIList parent, int width = 20) : base(new Vector2(parent.Dimensions.X - width, 0), new Vector2(width, parent.Dimensions.Y), parent)
        {
        }

        /// <summary>
        /// updates the size the parent would be if it were fully expanded. Always smaller than
        /// its size as defined in its width and height
        /// </summary>
        public void ChangeTotalElementsSize()
        {
            if (parent.Children.Count > 0)
                totalElementsSize = parent.Children[parent.Children.Count - 1].AbsoluteRectangle.Bottom - parent.Children[0].AbsoluteRectangle.Top;
            else
                totalElementsSize = parent.AbsoluteRectangle.Height;
            if (totalElementsSize < AbsoluteRectangle.Height)
            {
                totalElementsSize = AbsoluteRectangle.Height;
            }

        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //fills the solidcolortexture with a single white color
            if (solidColorTexture == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = Color.White;
                solidColorTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                solidColorTexture.SetData(colordata);
            }
            //draws the scrollbar and the bar in their respective colors with the bar on top
            spriteBatch.Draw(solidColorTexture, OriginLocationRectangle, backColor);
            spriteBatch.Draw(solidColorTexture, OriginLocationBarRectangle, barColor);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //use the input if it isn't used already and is hovering over the scrollbar
            if (!parent.InputEaten && AbsoluteRectangle.Contains(inputHelper.MousePosition))
            {
                parent.InputEater = this;
                //if we press the mouse button on the bar we start draggin
                if (inputHelper.MouseLeftButtonDown() && AbsoluteBarRectangle.Contains(inputHelper.MousePosition))
                {
                   mouseStartDragLocation = inputHelper.MousePosition.ToPoint();
                   scolldistanceStartDrag = ScrollDistance;
                   beingDragged = true;
                }
            }
            else if (beingDragged)
            {
                if (inputHelper.MouseLeftButtonDown())
                {
                    //the scrolldistance is the scroll distance we started with plus the distance the mouse has
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.Invalidate = true;
        }

        /// <summary>
        /// if we are dragging we want to keep using the input
        /// </summary>
        public override bool WantsToEatInput
        {
            get
            {
                return beingDragged;
            }
        }
    }
}
