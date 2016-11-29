using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class ListButton : Button
    {
        //this variable has to be publicly changeable in case we want to display a list with a default option
        protected bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }



        private Texture2D selectedBackground;
        private Vector2 anchorPoint;

        public ListButton(ContentManager content, string text, Vector2 myLocation, UIList, parentLocation, int width, int index, ClickEventHandler onClick, string backgroundName = "floor", string hoverBackgroundName = "key", string selectedBackgroundName = "end_door", string textFont = "menufont") : base(content, text, myLocation, onClick, backgroundName, hoverBackgroundName, textFont)
        {
            this.rectangle.Width = width;
            this.anchorPoint = parentLocation;
            this.index = index;
            this.selectedBackground = content.Load<Texture2D>(selectedBackgroundName);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!selected)
                base.Draw(gameTime, spriteBatch);
            else
            {
                spriteBatch.Draw(selectedBackground, rectangle, Color.White);
                spriteBatch.DrawString(spriteFont, text, rectangle.Location.ToVector2(), Color.White);
            }
        }

        //a list button always remembers its position in a list, so it is of paramount importance to normalize it's location temporarily when no
        public override void HandleInput(InputHelper inputHelper)
        {
            //we need to normalize our location
            this.rectangle.Location = (rectangle.Location.ToVector2() + parentLocation).ToPoint();
            if (!selected)
                base.HandleInput(inputHelper);
            //go back to local coordinates to ensure we don't draw out of our box
            this.rectangle.Location = (rectangle.Location.ToVector2() - parentLocation).ToPoint();

        }
    }
}
