using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame.Gameplay.UIObjects
{
    public class UIContainer : UIObject
    {
        //list of the uiobjects contained in the container
        protected UIObjectList<UIObject> children;

        //when the input is permamenteaten the input doesn't reset after an inputloop
        private UIObject inputEater = null;

        /// <summary>
        /// returns which object is using the input.
        /// seet which object is using the input.
        /// because in each UI hirarchy we only want one object to use the input we call the parent
        /// </summary>
        public UIObject InputEater
        {
            get { if (parent == null)
                    return this.inputEater;
                return parent.inputEater;
                }
            set
            {
                if (parent == null)
                    this.inputEater = value;
                else
                    parent.inputEater = value;
            }
        }

        //this method is for the children. Every child might have a different anchor point in a sorted list.
        public virtual Vector2 GetChildAnchorPoint(UIObject uiObject)
        {
            return Vector2.Zero;
        }

        public UIObjectList<UIObject> Children
        {
            get { return children; }
        }

        public Boolean InputEaten
        {
            get { return InputEater != null; }
        }

        /// <summary>
        /// contains UI elements
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        public UIContainer(Vector2 location, Vector2 dimensions, UIContainer parent) : base(location, dimensions, parent)
        {
            children = new UIObjectList<UIObject>();
        }

        public void AddChild(UIObject child)
        {
            children.Add(child);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //we check if the inputEater wants to keep eating our input
            //we can safely ask if the inputeater wants to use the input because inputeaten checks
            //if an element is eating the input
            if (parent == null && InputEaten && !InputEater.WantsToEatInput)
            {
                InputEater = null;
            }

            children.HandleInput(inputHelper);

            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            children.Update(gameTime);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            children.Draw(gameTime, spriteBatch);
        }

        public override void Reset()
        {
        }
    }
}
