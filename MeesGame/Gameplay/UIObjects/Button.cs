using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    public class Button : UIObject
    {
        public delegate void ClickEventHandler(Button button);
        public event ClickEventHandler OnClick;

        /// <summary>
        /// text = the text the button displays
        /// spritefont = the font used for the image
        /// background = the background of the button
        /// hoverBackground = the background used when the mouse hovers over it
        /// selectedBackground = the background the button takes when it is in selected state
        /// </summary>
        protected String text;
        protected SpriteFont spriteFont;
        protected Texture2D background;
        protected Texture2D hoverBackground;
        private Texture2D selectedBackground;

        /// <summary>
        /// Indicates if the button is selected, for example when choosing a file in the fileExplorer
        /// </summary>
        protected bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// method used to create a customizable button
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        /// <param name="text">text the button displays</param>
        /// <param name="onClick">action when the button is pressed</param>
        /// <param name="autoDimensions">scale the button automatically to the size of the text</param>
        /// <param name="hideOverflow">when autodimensions is off and the dimensions smaller than the text, hide the excess text</param>
        /// <param name="backgroundName">the background for the button by its name in the contentmanager</param>
        /// <param name="hoverBackgroundName">the texture that overlays the background when the mouse is hovering over the button</param>
        /// <param name="selectedBackgroundName">the texture that overlays the background when the button is selected</param>
        /// <param name="textFont">the name of the textfont used for the text as in the contenmanager</param>
        public Button(Vector2 location, Vector2 dimensions, UIContainer parent, string text, ClickEventHandler onClick, bool autoDimensions = true, string backgroundName = "floorTile", string hoverBackgroundName = "keyOverlay", string selectedBackgroundName = "horizontalEnd", string textFont = "menufont") : base(location, dimensions, parent)
        {
            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(textFont);
            background = GameEnvironment.AssetManager.Content.Load<Texture2D>(backgroundName);
            hoverBackground = GameEnvironment.AssetManager.Content.Load<Texture2D>(hoverBackgroundName);
            selectedBackground = GameEnvironment.AssetManager.Content.Load<Texture2D>(selectedBackgroundName);

            this.text = text;
            if (autoDimensions)
                this.Dimensions = spriteFont.MeasureString(text);
            OnClick += onClick;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, OriginLocationRectangle, Color.White);
            if (selected)
                spriteBatch.Draw(selectedBackground, OriginLocationRectangle, Color.White);
            else if (Hovering)
                spriteBatch.Draw(hoverBackground, OriginLocationRectangle, Color.White);
            spriteBatch.DrawString(spriteFont, text, Vector2.Zero, Color.White);
        }

        /// <summary>
        /// invalidates the button every frame because we need to test if the mouse is hovering
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Invalidate = true;
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
