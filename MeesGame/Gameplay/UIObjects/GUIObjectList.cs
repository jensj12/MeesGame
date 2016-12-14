﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace MeesGame
{
    public class GUIObjectList<Type> : GameObject, IList<Type> where Type : GUIObject
    {
        protected List<Type> children = new List<Type>();

        public GUIObjectList()
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
            foreach (Type obj in children)
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
            List<Type>.Enumerator e = children.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Draw(gameTime, spriteBatch);
            }
        }

        public override void Reset()
        {
            base.Reset();
            foreach (Type obj in children)
            {
                obj.Reset();
            }
        }

        public void Clear()
        {
            ((ICollection<Type>)children).Clear();
        }

        public bool Contains(Type item)
        {
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
            ((IList<Type>)children).RemoveAt(index);
        }
    }
}