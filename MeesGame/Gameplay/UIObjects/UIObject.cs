using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame.Gameplay.UIObjects
{
    abstract class UIObject : IGameLoopObject
    {
        private Vector2 location;
        private Vector2 dimensions;
        private UIObject parent;
        private UIObjectList uiObjects;

        public Vector2 Location
        {
            get { return location; }
            set { location = value;}
        }

        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Rectangle Rectangle {
            get { return new Rectangle(location.ToPoint(), dimensions.ToPoint()); }
        }

        public UIObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public virtual void HandleInput(InputHelper inputHelper)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public virtual void Reset()
        {

        }
    }
}
