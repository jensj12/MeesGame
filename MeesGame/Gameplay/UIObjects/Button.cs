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

        /// <summary>
        /// method used to create a customizable button
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        /// <param name="content"></param>
        /// <param name="text">text the button displays</param>
        /// <param name="onClick">action when the button is pressed</param>
        /// <param name="autoDimensions">scale the button automatically to the size of the text</param>
        /// <param name="hideOverflow">when autodimensions is off and the dimensions smaller than the text, hide the excess text</param>
        /// <param name="hoverBackgroundName">the texture that overlays the background when the mouse is hovering over the button</param>
        /// <param name="selectedBackgroundName">the texture that overlays the background when the button is selected</param>
        /// <param name="textFont">the name of the textfont used for the text as in the contenmanager</param>
        public Button(Vector2 location, Vector2 dimensions, UIContainer parent, string text, ClickEventHandler onClick, bool autoDimensions = true, bool hideOverflow = false, string backgroundName = "floorTile", string hoverBackgroundName = "key", string selectedBackgroundName = "end_door", string textFont = "menufont") : base(location, dimensions, parent, hideOverflow)
        {
            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(textFont);
            background = GameEnvironment.AssetManager.Content.Load<Texture2D>(backgroundName);
            hoverBackground = GameEnvironment.AssetManager.Content.Load<Texture2D>(hoverBackgroundName);
            selectedBackground = GameEnvironment.AssetManager.Content.Load<Texture2D>(selectedBackgroundName);

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
