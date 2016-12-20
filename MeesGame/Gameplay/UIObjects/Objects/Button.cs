using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MeesGame
{
    public class Button : UIObject
    {
        /// <summary>
        /// Text = the text the button displays
        /// Spritefont = the font used for the image
        /// backgroundAndOverlays = the background of the button can consist of multiple spritesheets
        /// because it needs to be rendered like tiles. The multiple textures are rendered in the order they are stored in the list 
        /// HoverBackground = the background used when the mouse hovers over it
        /// SelectedBackground = the background the button takes when it is in selected state
        /// </summary>
        protected String text;
        protected SpriteFont spriteFont;
        protected List<SpriteSheet> background;
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
        /// <param name="text">Text the button displays</param>
        /// <param name="onClick">Action when the button is pressed</param>
        /// <param name="autoDimensions">Scale the button automatically to the size of the text</param>
        /// <param name="hideOverflow">When autodimensions is off and the dimensions smaller than the text, hide the excess text</param>
        /// <param name="backgroundName">The background for the button by its name in the contentmanager</param>
        /// <param name="hoverBackgroundName">The texture that overlays the background when the mouse is hovering over the button</param>
        /// <param name="selectedBackgroundName">The texture that overlays the background when the button is selected</param>
        /// <param name="textFont">The name of the text font used for the text as in the contenmanager</param>
        /// <param name="overlays">The overlays that should be rendered over the background</param>
        public Button(Vector2? location, Vector2? dimensions, string text, OnClickEventHandler onClick = null, string backgroundName = "floorTile", string hoverBackgroundName = "keyOverlay", string selectedBackgroundName = "horizontalEnd", string textFont = "menufont", string[] overlayNames = null) : base(location, dimensions)
        {
            background = new List<SpriteSheet>();

            spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(textFont);
            if (backgroundName != null)
                background.Add(new SpriteSheet(backgroundName));
            hoverBackground = new SpriteSheet(hoverBackgroundName);
            selectedBackground = new SpriteSheet(selectedBackgroundName);
            if (overlayNames != null)
                for (int i = 0; i < overlayNames.Length; i++)
                    background.Add(new SpriteSheet(overlayNames[i]));

            this.text = text;

            Dimensions = dimensions ?? Vector2.Zero;

            if (onClick != null)
                Click += onClick;
        }

        /// <summary>
        /// changes the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="v"></param>
        public void UpdateText(string text, bool v)
        {
            this.text = text;
        }

        /// <summary>
        /// changes the dimensions of the button. if set to Vector2.Zero it measures the string
        /// </summary>
        public override Vector2 Dimensions
        {
            get
            {
                return base.Dimensions;
            }

            set
            {
                Vector2 measuredDimensions = spriteFont.MeasureString(text);
                if (value == Vector2.Zero)
                    base.Dimensions = measuredDimensions;
                else
                    base.Dimensions = value;
            }
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawTask(gameTime, spriteBatch);
            for (int i = 0; i < background.Count; i++)
                background[i].Draw(spriteBatch, OriginLocationRectangle.Location.ToVector2(), Vector2.Zero, (int)Dimensions.X, (int)Dimensions.Y);

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
            Invalidate();
            base.Update(gameTime);
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
    }
}