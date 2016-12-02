using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    public class Button : UIObject
    {
        public delegate void ClickEventHandler(Button button);
        public event ClickEventHandler OnClick;

        protected String text;
        protected SpriteFont spriteFont;
        protected Texture2D background;
        protected Texture2D hoverBackground;

        protected bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        private Texture2D selectedBackground;

        public Button(Vector2 location, Vector2 dimensions, UIContainer parent, ContentManager content, string text, ClickEventHandler onClick, bool autoDimensions = true, bool hideOverflow = false, string backgroundName = "floorTile", string hoverBackgroundName = "key", string selectedBackgroundName = "end_door", string textFont = "menufont") : base(location, dimensions, parent, hideOverflow)
        {
            spriteFont = content.Load<SpriteFont>(textFont);
            background = content.Load<Texture2D>(backgroundName);
            hoverBackground = content.Load<Texture2D>(hoverBackgroundName);
            selectedBackground = content.Load<Texture2D>(selectedBackgroundName);

            this.text = text;
            if (autoDimensions)
                this.dimensions = spriteFont.MeasureString(text);
            OnClick += onClick;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Rectangle, Color.White);
            if (selected)
                spriteBatch.Draw(selectedBackground, Rectangle, Color.White);
            else if (Hovering)
                spriteBatch.Draw(hoverBackground, Rectangle, Color.White);
            spriteBatch.DrawString(spriteFont, text, Location, Color.White);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (Clicked)
            {
                OnClick?.Invoke(this);
            }

        }
    }
}
