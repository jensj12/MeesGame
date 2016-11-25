using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MeesGame
{
    class Button : GameObject
    {
        public delegate void ClickEventHandler(Object o);
        public event ClickEventHandler OnClick;

        protected String text;
        protected Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        protected SpriteFont spriteFont;
        protected bool hovering = false;
        protected Texture2D background;
        protected Texture2D hoverBackground;

        public Button(ContentManager content, String text, Vector2 location, ClickEventHandler onClick, string backgroundName = "floor", string hoverBackgroundName = "key", string textFont = "menufont")
        {
            spriteFont = content.Load<SpriteFont>(textFont);
            background = content.Load<Texture2D>(backgroundName);
            hoverBackground = content.Load<Texture2D>(hoverBackgroundName);
            this.text = text;
            Vector2 dimen = spriteFont.MeasureString(text);
            this.rectangle = new Rectangle(location.ToPoint(), dimen.ToPoint());
            OnClick += onClick;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, rectangle, Color.White);
            if (hovering)
                spriteBatch.Draw(hoverBackground, rectangle, Color.White);
            spriteBatch.DrawString(spriteFont, text, rectangle.Location.ToVector2(), Color.White);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (rectangle.Contains(inputHelper.MousePosition))
            {
                hovering = true;
                if (inputHelper.MouseLeftButtonPressed())
                {
                    OnClick?.Invoke(this);
                }
            }
            else
            {
                hovering = false;
            }

        }

    }
}
