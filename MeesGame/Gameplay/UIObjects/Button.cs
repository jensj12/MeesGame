using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class Button : UIObject
    {
        public delegate void ClickEventHandler(Button button);
        public event ClickEventHandler OnClick;

        /// <summary>
        /// Text = the text the button displays
        /// Spritefont = the font used for the image
        /// Background = the background of the button
        /// HoverBackground = the background used when the mouse hovers over it
        /// SelectedBackground = the background the button takes when it is in selected state
        /// </summary>
        protected String text;
        protected SpriteFont spriteFont;
        protected SpriteSheet background;
        protected SpriteSheet hoverBackground;
        private SpriteSheet selectedBackground;

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
        /// Method used to create a customizable button
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        /// <param name="text">Text the button displays</param>
        /// <param name="onClick">Action when the button is pressed</param>
        /// <param name="autoDimensions">Scale the button automatically to the size of the text</param>
        /// <param name="hideOverflow">When autodimensions is off and the dimensions smaller than the text, hide the excess text</param>
        /// <param name="backgroundName">The background for the button by its name in the contentmanager</param>
        /// <param name="hoverBackgroundName">The texture that overlays the background when the mouse is hovering over the button</param>
        /// <param name="selectedBackgroundName">The texture that overlays the background when the button is selected</param>
        /// <param name="textFont">The name of the text font used for the text as in the contenmanager</param>
        public Button(Vector2 location, Vector2 dimensions, UIContainer parent, string text, ClickEventHandler onClick, bool autoDimensions = true, string backgroundName = "floorTile", string hoverBackgroundName = "keyOverlay", string selectedBackgroundName = "horizontalEnd", string textFont = "menufont") : base(location, dimensions, parent)
        {
            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(textFont);
            background = new SpriteSheet(backgroundName);
            hoverBackground = new SpriteSheet(hoverBackgroundName);
            selectedBackground = new SpriteSheet(selectedBackgroundName);

            this.text = text;
            if (autoDimensions)
                this.Dimensions = spriteFont.MeasureString(text);
            else
                this.Dimensions = dimensions;
            OnClick += onClick;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch, OriginLocationRectangle.Location.ToVector2(), Vector2.Zero, (int)Dimensions.X, (int)Dimensions.Y);
            if (selected)
                selectedBackground.Draw(spriteBatch, OriginLocationRectangle.Location.ToVector2(), Vector2.Zero, (int)Dimensions.X, (int)Dimensions.Y);
            else if (Hovering)
                hoverBackground.Draw(spriteBatch, OriginLocationRectangle.Location.ToVector2(), Vector2.Zero, (int)Dimensions.X, (int)Dimensions.Y);
            spriteBatch.DrawString(spriteFont, text, Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Invalidates the button every frame because we need to test if the mouse is hovering
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
