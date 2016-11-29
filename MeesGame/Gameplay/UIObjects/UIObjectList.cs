using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame.Gameplay.UIObjects
{
    class UIObjectList : GameObject
    {
        protected List<UIObject> children;

        public UIObjectList()
        {
        }

        public List<UIObject> Children
        {
            get { return children; }
        }

        public void Add(UIObject obj)
        {
            children.Add(obj);
        }

        public void Remove(UIObject obj)
        {
            children.Remove(obj);
            obj.Parent = null;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                children[i].HandleInput(inputHelper);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (UIObject obj in children)
            {
                obj.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible)
            {
                return;
            }
            List<UIObject>.Enumerator e = children.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Draw(gameTime, spriteBatch);
            }
        }

        public override void Reset()
        {
            base.Reset();
            foreach (UIObject obj in children)
            {
                obj.Reset();
            }
        }
    }
}