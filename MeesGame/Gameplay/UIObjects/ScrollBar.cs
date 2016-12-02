﻿using Microsoft.Xna.Framework;
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
        private int ScrollDistance
        {
            get
            {
                if (totalElementsSize - Rectangle.Height != 0)
                    return ((UIList)parent).ElementsOffset * (Rectangle.Height - (int)(Rectangle.Height / (double)totalElementsSize * Rectangle.Height)) / (totalElementsSize - Rectangle.Height);
                return 0;
            }
            set
            {
                if (totalElementsSize - Rectangle.Height != 0)
                    ((UIList)parent).ElementsOffset = (int)(value * (totalElementsSize - Rectangle.Height) / (double)(Rectangle.Height - BarRectangle.Height));
                else
                    ((UIList)parent).ElementsOffset = 0;
            }
        }
        //a solid color is inserted into this texture for painting a solid color
        private Texture2D emptyTexture;

        public Rectangle BarRectangle
        {
            get
            {
                if (totalElementsSize <= Rectangle.Height)
                    return Rectangle;
                else
                {
                    return new Rectangle(Rectangle.X, Rectangle.Y + ScrollDistance, Rectangle.Width, (int)(Rectangle.Height / (double)totalElementsSize * Rectangle.Height));
                }
            }
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
            if (emptyTexture == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = Color.White;
                emptyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                emptyTexture.SetData(colordata);
            }
            spriteBatch.Draw(emptyTexture, Rectangle, backColor);
            spriteBatch.Draw(emptyTexture, BarRectangle, barColor);
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