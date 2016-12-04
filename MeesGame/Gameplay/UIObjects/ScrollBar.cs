using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class ScrollBar : UIObject
    {
        private readonly Color barColor = Color.Aqua;
        private readonly Color backColor = Color.Beige;
        //stores the total length of the scrollbar.
        private int totalElementsSize;
        //initialize scrollbar on top
        private int scolldistanceStartDrag;

        //a solid color is inserted into this texture for painting a solid color
        private Texture2D solidColorTexture;
        
        /// <summary>
        /// The distance from the scrollbar to the top of the parent element.
        /// </summary>
        private int ScrollDistance
        {
            get
            {
                //if the scrollbar fills the entire parent height, it cannot be moved.
                if (totalElementsSize <= Rectangle.Height) return 0;
                else
                    ///a simple way to picture this calculation is to think that the maximum distance of the elementsoffset is equal to totalElementsSize - Rectangle.Height 
                    ///and the maximum distance the scrollbar can travel is Rectangle.Height - Barheight, so it will always remain between those bounds and linearly scale
                    ///offset distance
                    return ((UIList)parent).ElementsOffset * (Rectangle.Height - BarHeight) / (totalElementsSize - Rectangle.Height);
            }
            set
            {
                if (totalElementsSize != Rectangle.Height)
                    ///the ratio between the maximum distance of the elementsoffset * the maximum distance the scrollbar can travel, and multiply this by a number between zero
                    ///and the maximum distance the scrollbar can travel
                    ///
                    ((UIList)parent).ElementsOffset = (int)(value * (totalElementsSize - Rectangle.Height) / (double)(Rectangle.Height - BarRectangle.Height));
                else
                    ((UIList)parent).ElementsOffset = 0;
            }
        }

        public Rectangle BarRectangle
        {
            get
            {
                if (totalElementsSize <= Rectangle.Height)
                    return Rectangle;
                else
                {
                    return new Rectangle(Rectangle.X, Rectangle.Y + ScrollDistance, Rectangle.Width, BarHeight);
                }
            }
        }

        private int BarHeight
        {
            get { return (int)(Rectangle.Height / (double)totalElementsSize * Rectangle.Height); }
        }

        private Point mouseStartDragLocation = new Point();
        private bool beingDragged = false;
        public bool BeingDragged
        {
            get { return beingDragged; }
        }

        private int width;


        public ScrollBar(UIList parent, int width = 20) : base(new Vector2(parent.Dimensions.X - width, 0), new Vector2(width, parent.Dimensions.Y), parent)
        {
            this.width = width;
        }

        public override Vector2 Location
        {
            get
            {
                return parent.Location + new Vector2(parent.Dimensions.X - width, 0);
            }
        }

        public void ChangeTotalElementsSize()
        {
            if (parent.Children.Count > 0)
                totalElementsSize = parent.Children[parent.Children.Count - 1].Rectangle.Bottom - parent.Children[0].Rectangle.Top;
            else
                totalElementsSize = parent.Rectangle.Height;
            if (totalElementsSize < Rectangle.Height)
            {
                totalElementsSize = Rectangle.Height;
            }

        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (solidColorTexture == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = Color.White;
                solidColorTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                solidColorTexture.SetData(colordata);
            }
            spriteBatch.Draw(solidColorTexture, Rectangle, backColor);
            spriteBatch.Draw(solidColorTexture, BarRectangle, barColor);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (!parent.InputEaten && Rectangle.Contains(inputHelper.MousePosition))
            {
                parent.InputEater = this;
                if (inputHelper.MouseLeftButtonDown())
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
                    //distance the mouse has moved since initially held down
                    ScrollDistance = scolldistanceStartDrag + (int)(inputHelper.MousePosition.Y - mouseStartDragLocation.Y);
                    if (BarRectangle.Bottom > Rectangle.Bottom)
                        ScrollDistance = Rectangle.Height - BarRectangle.Height;
                    if (ScrollDistance < 0)
                        ScrollDistance = 0;
                }
                else
                {
                    beingDragged = false;
                }
            }
        }

        //the scrollbar wants to eat untill we drop the mousebutton
        public override bool WantsToEatInput
        {
            get
            {
                return beingDragged;
            }
        }
    }
}
