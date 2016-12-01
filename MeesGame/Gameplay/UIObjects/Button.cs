using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MeesGame
{
    public class Button : UIObject
    {
        public delegate void ClickEventHandler(Object o);
        public event ClickEventHandler OnClick;

        protected String text;
        protected SpriteFont spriteFont;
        protected Texture2D background;
        protected Texture2D hoverBackground;

        public Button(Vector2 location, Vector2 dimensions, UIObject parent, ContentManager content, String text, ClickEventHandler onClick = null, bool autoDimensions = true, bool hideOverflow = false, string backgroundName = "floor", string hoverBackgroundName = "key", string textFont = "menufont") : base(location, dimensions, parent, hideOverflow)
        {
            spriteFont = content.Load<SpriteFont>(textFont);
            background = content.Load<Texture2D>(backgroundName);
            hoverBackground = content.Load<Texture2D>(hoverBackgroundName);
            this.text = text;
            if (autoDimensions)
                this.dimensions = spriteFont.MeasureString(text);
            OnClick += onClick;
        }

        public override void DrawSelf(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Rectangle, Color.White);
            if (hovering)
                spriteBatch.Draw(hoverBackground, Rectangle, Color.White);
            spriteBatch.DrawString(spriteFont, text, Location, Color.White);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (hovering)
            {
                if (inputHelper.MouseLeftButtonPressed())
                {
                    OnClick?.Invoke(this);
                }
            }

        }
    }
}
