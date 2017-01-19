using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MeesGame
{
    public class Textbox : UIComponent, IMeasuredDimensions
    {
        /// <summary>
        /// The text the text-box displays.
        /// </summary>
        protected string text;

        /// <summary>
        /// The font used to render the text with.
        /// </summary>
        protected SpriteFont spriteFont;

        /// <summary>
        /// Caches the spritefont.measure() method.
        /// </summary>
        private Vector2 cachedMeasuredDimensions;

        /// <summary>
        /// Color the text is displayed in.
        /// </summary>
        protected Color textColor;

        /// <summary>
        /// Creates a textbox
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="text">Text that will be displayed by the textbox.</param>
        /// <param name="spritefont">Font used the render the text with.</param>
        /// <param name="textColor">Color the text is displayed in.</param>
        public Textbox(Location location, Dimensions dimensions, String text, string spritefont = null, Color? textColor = null) : base(location, dimensions)
        {
            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(spritefont ?? DefaultUIValues.Default.SpriteFont);
            Text = text;
            this.textColor = textColor ?? Color.White;

            if (dimensions == null)
                Dimensions = new MeasuredDimensions(this);
        }

        /// <summary>
        /// Dimensions as measured by the spritefont.
        /// </summary>
        public Vector2 MeasuredDimensions()
        {
            return cachedMeasuredDimensions;
        }

        /// <summary>
        /// Updates the dimensions measured by the spritefont
        /// </summary>
        private void UpdateCachedMeasuredDimensions()
        {
            cachedMeasuredDimensions = spriteFont.MeasureString(InternalText);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            spriteBatch.DrawString(spriteFont, InternalText, anchorPoint, textColor);
        }

        protected virtual string InternalText
        {
            get { return Text; }
        }

        /// <summary>
        /// The text the text-box displays
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                UpdateCachedMeasuredDimensions();
                Invalidate();
            }
        }
    }
}
