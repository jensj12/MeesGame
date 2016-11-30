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
        public delegate void ScrollDistance(int distance);
        private readonly Color barColor = Color.Aqua;
        private readonly Color backColor = Color.Beige;
        //stores the total length of the scrollbar.
        private int totalElementsSize;
        //initialize scrollbar on top
        private int scolldistanceStartDrag;
        private int scrolldistance = 0;
        private ScrollDistance callback;
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
                    return new Rectangle(Rectangle.X, Rectangle.Y + scrolldistance, Rectangle.Width, (int)(Rectangle.Height / (double)totalElementsSize * Rectangle.Height));
                }
            }
        }
        private Point mouseStartDragLocation = new Point();
        private bool beingDragged = false;
        public bool BeingDragged
        {
            get { return beingDragged; }
        }
        //this class has it's own inputhelper because it needs to keep scrolling while dragged
        //regardless of the mouseposition
        InputHelper inputHelper;



        public ScrollBar(UIList parent, int totalElementsHeight, ScrollDistance action, int width = 20) : base(new Vector2(parent.Dimensions.X - width, 0), new Vector2(width, parent.Dimensions.Y), parent)
        {
            this.totalElementsSize = totalElementsHeight;
            this.callback = action;
        }

        public override void DrawSelf(GameTime gameTime, SpriteBatch spriteBatch)
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
            base.HandleInput(inputHelper);
            this.inputHelper = inputHelper;
            if (!beingDragged && inputHelper.MouseLeftButtonDown() && hovering)
            {
                mouseStartDragLocation = inputHelper.MousePosition.ToPoint();
                scolldistanceStartDrag = scrolldistance;
                beingDragged = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (beingDragged)
            {
                inputHelper.Update();

                if (inputHelper.MouseLeftButtonDown())
                {
                    scrolldistance = scolldistanceStartDrag + (int)(inputHelper.MousePosition.Y - mouseStartDragLocation.Y);
                    if (BarRectangle.Bottom > Rectangle.Bottom)
                        scrolldistance = Rectangle.Height - BarRectangle.Height;
                    if (BarRectangle.Top < Rectangle.Top)
                        scrolldistance = 0;
                    callback(scrolldistance);
                }
                else
                {
                    beingDragged = false;
                }
            }

        }
    }
}
