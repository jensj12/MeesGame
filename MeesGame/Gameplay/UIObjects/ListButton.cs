using MeesGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class ListButton : Button
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

        public ListButton(Vector2 location, Vector2 dimensions, UIList parent, ContentManager content, string text, int index, ClickEventHandler onClick, string backgroundName = "floor", string hoverBackgroundName = "key", string selectedBackgroundName = "end_door", string textFont = "menufont") : base(location, dimensions, parent, content, text, onClick)
        {
            this.index = index;
            this.selectedBackground = content.Load<Texture2D>(selectedBackgroundName);
        }

        public override Vector2 Location
        {
            get { return base.Location - new Vector2(0, ParentList.ElementsOffset); }
        }

        public override Vector2 RelativeLocation
        {
            get { return location + new Vector2(0, ParentList.ElementsOffset); }
        }

        public UIList ParentList
        {
            get
            {
                return (UIList)base.Parent;
            }

            set
            {
                base.Parent = value;
            }
        }

        public override void DrawSelf(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!selected)
                base.DrawSelf(gameTime, spriteBatch);
            else
            {
                spriteBatch.Draw(selectedBackground, Rectangle, Color.White);
                spriteBatch.DrawString(spriteFont, text, Location, Color.White);
            }
        }
    }
}
