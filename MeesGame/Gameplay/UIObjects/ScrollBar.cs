using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame.Gameplay.UIObjects
{
    public class ScrollBar : GameObject
    {
        public delegate void ScrollDistance(int distance);
        private readonly Color barColor = Color.Aqua;
        private readonly Color backColor = Color.Beige;
        private bool vertical;
        //this rectangles' location is the most right bottom corner because in that
        //way we can easily assign it to a location without worrying about details
        private Rectangle rectangle;
        public Rectangle EntireRectangle
        {
            get { return rectangle; }
        }
        //stores the total length of the scrollbar.
        private int totalElementsSize;
        //initialize scrollbar on top
        private int scolldistanceStartDrag;
        private int scrolldistance = 0;
        private ScrollDistance callback;
        private Texture2D emptyTexture;
        public Rectangle BarRectangle
        {
            get{
                if (vertical)
                {
                    if (totalElementsSize <= rectangle.Height)
                        return rectangle;
                    else
                    {
                        return new Rectangle(rectangle.X, rectangle.Y + scrolldistance, rectangle.Width, (int)(rectangle.Height / (double)totalElementsSize * rectangle.Height));
                    }
                }
                else
                {
                    if (totalElementsSize <= rectangle.Width)
                        return rectangle;
                    else
                    {
                        return new Rectangle(rectangle.X + scrolldistance, rectangle.Y, (int)(rectangle.Width / (double)totalElementsSize * rectangle.Width), rectangle.Height);
                    }
                }

            }
        }
        private Point mouseStartDragLocation = new Point();
        private bool beingDragged;
        public bool BeingDragged
        {
            get { return beingDragged; }
        }

        public ScrollBar(Point location, int height, int totalElementsHeight, ScrollDistance action, bool vertical = true, int width = 20)
        {
                this.rectangle = new Rectangle(location.X- width, location.Y - height, width, height);
            this.totalElementsSize = totalElementsHeight;
            this.callback = action;
            this.vertical = vertical;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (emptyTexture == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = Color.White;
                emptyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                emptyTexture.SetData(colordata);
            }
            spriteBatch.Draw(emptyTexture, rectangle, backColor);
            spriteBatch.Draw(emptyTexture, BarRectangle, barColor);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (beingDragged)
            {
                if (inputHelper.MouseLeftButtonDown())
                {
                    if (vertical)
                    {
                        scrolldistance = scolldistanceStartDrag + (int) (inputHelper.MousePosition.Y - mouseStartDragLocation.Y);
                        if (BarRectangle.Bottom > rectangle.Bottom)
                            scrolldistance = rectangle.Height - BarRectangle.Height;
                        if (BarRectangle.Top < rectangle.Top)
                            scrolldistance = 0;
                    }
                    else
                    {
                        scrolldistance = scolldistanceStartDrag + (int)(inputHelper.MousePosition.X - mouseStartDragLocation.X);
                        if (BarRectangle.Right > rectangle.Right)
                            scrolldistance = rectangle.Width - BarRectangle.Width;
                        if (BarRectangle.Left < rectangle.Left)
                            scrolldistance = 0;


                    }
                    callback(scrolldistance);
                } else
                {
                    beingDragged = false;
                }
            }
            else if (inputHelper.MouseLeftButtonDown() && BarRectangle.Contains(inputHelper.MousePosition))
            {
                mouseStartDragLocation = inputHelper.MousePosition.ToPoint();
                scolldistanceStartDrag = scrolldistance;
                beingDragged = true;
            }
            else
                base.HandleInput(inputHelper);
        }
    }
}
