using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;

namespace MeesGame
{
    public class ComponentList<Type> : IList<Type> where Type : UIComponent
    {
        protected List<Type> children = new List<Type>();

        public ComponentList()
        {
        }

        public int Count
        {
            get
            {
                return ((ICollection<Type>)children).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<Type>)children).IsReadOnly;
            }
        }

        public Type this[int index]
        {
            get
            {
                return ((IList<Type>)children)[index];
            }

            set
            {
                ((IList<Type>)children)[index] = value;
            }
        }

        public void Add(Type obj)
        {
            children.Add(obj);
        }

        public void Remove(Type obj)
        {
            children.Remove(obj);
            obj.Dispose();
        }

        public void UpdateBounds()
        {
            for (int i = 0; i < children.Count; i++)
                children[i].RefreshCachedBounds();
        }

        public void HandleInput(InputHelper inputHelper)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                children[i].HandleInput(inputHelper);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Type obj in children)
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            List<Type>.Enumerator e = children.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Draw(gameTime, spriteBatch, anchorPoint);
            }
        }

        public void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            List<Type>.Enumerator e = children.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.RenderTexture(gameTime, spriteBatch);
            }
        }

        public void Reset()
        {
            foreach (Type obj in children)
            {
                obj.Reset();
            }
        }

        public void Dispose()
        {
            foreach (Type obj in children)
            {
                obj.Dispose();
            }
        }

        public void Clear()
        {
            Dispose();
            ((ICollection<Type>)children).Clear();
        }

        public bool Contains(Type item)
        {
            foreach (Type child in children)
            {
                if (child.ContainsComponent(item))
                    return true;
            }
            return ((ICollection<Type>)children).Contains(item);
        }

        public void CopyTo(Type[] array, int arrayIndex)
        {
            ((ICollection<Type>)children).CopyTo(array, arrayIndex);
        }

        bool ICollection<Type>.Remove(Type item)
        {
            return ((ICollection<Type>)children).Remove(item);
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return ((ICollection<Type>)children).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Type>)children).GetEnumerator();
        }

        public int IndexOf(Type item)
        {
            return ((IList<Type>)children).IndexOf(item);
        }

        public void Insert(int index, Type item)
        {
            ((IList<Type>)children).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            children[index].Dispose();
            ((IList<Type>)children).RemoveAt(index);
        }
    }
}
