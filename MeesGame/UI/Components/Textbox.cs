using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MeesGame
{
    public class Textbox : UIComponent, IMeasuredDimensions
    {
        /// <summary>
        /// The text the text-box displays
        /// </summary>
        protected string text;

        /// <summary>
        /// The font used to render the text with
        /// </summary>
        protected SpriteFont spriteFont;

        public Textbox(Location location, Dimensions dimensions, String text, string spritefont = null) : base(location, dimensions)
        {
            this.text = text;
            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(spritefont ?? DefaultUIValues.Default.DefaultSpriteFont);
            if (dimensions == null)
                Dimensions = new MeasuredDimensions(this);
        }

        /// <summary>
        /// Dimensions as measured by the spritefont.
        /// </summary>
        public Vector2 MeasuredDimensions()
        {
            return spriteFont.MeasureString(InternalText);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            spriteBatch.DrawString(spriteFont, InternalText, anchorPoint, Color.White);
        }

        protected virtual String InternalText
        {
            get { return Text; }
            set { Text = value; }
        }

        /// <summary>
        /// The text the text-box displays
        /// </summary>
        public String Text
        {
            get { return text; }
            set
            {
                text = value;
                Invalidate();
            }
        }
    }
}
