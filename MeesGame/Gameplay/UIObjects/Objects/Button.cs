using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class Button : GUIObject
    {
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
        public Button(Vector2? location, Vector2? dimensions, string text, ClickEventHandler onClick = null, string backgroundName = "floorTile", string hoverBackgroundName = "keyOverlay", string selectedBackgroundName = "horizontalEnd", string textFont = "menufont") : base(location, dimensions)
        {
            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(textFont);
            background = new SpriteSheet(backgroundName);
            hoverBackground = new SpriteSheet(hoverBackgroundName);
            selectedBackground = new SpriteSheet(selectedBackgroundName);

            this.text = text;
            Vector2 measuredDimensions = spriteFont.MeasureString(text);
            this.Dimensions = dimensions ?? measuredDimensions;

            //we test if the dimensions vector contains a value < 0, if it contains one it replaces the value
            //with the value the text specifies
            this.Dimensions = new Vector2((Dimensions.X > 0) ? Dimensions.X : measuredDimensions.X,
                (Dimensions.Y > 0) ? Dimensions.Y : measuredDimensions.Y);
            if(onClick != null)
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
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }


        public override bool Invalidate
        {
            get
            {
                return base.Invalidate;
            }

            set
            {
                base.Invalidate = true;
            }
        }
    }
}
