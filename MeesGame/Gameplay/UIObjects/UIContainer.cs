using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame.Gameplay.UIObjects
{
    public class UIContainer : UIObject
    {
        protected UIObjectList<UIObject> children;

        //when the input is permamenteaten the input doesn't reset after an inputloop
        private UIObject inputEater = null;

        public UIObject InputEater
        {
            get
            {
                if (parent == null)
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

        //we use this boolean if we have our overflow hidden and the cursor is out to show that our children can't use this input
        private bool inputStopped = false;

        public bool InputEaten
        {
            get { return InputEater != null || inputStopped; }
        }

        public UIContainer(Vector2 location, Vector2 dimensions, UIContainer parent, bool hideOverflow = false) : base(location, dimensions, parent, hideOverflow)
        {
            children = new UIObjectList<UIObject>();
        }

        //this method is for the children. Every child might have a different anchor point in a sorted list.
        public virtual Vector2 GetChildAnchorPoint(UIObject uiObject)
        {
            return this.Location;
        }

        public UIObjectList<UIObject> Children
        {
            get { return children; }
        }

        public void AddChild(UIObject child)
        {
            children.Add(child);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //if our overflow is hidden we don't want our children to use the input
            inputStopped = false;

            //we check if the inputEater wants to keep eating our input
            if (parent == null && InputEaten && !InputEater.WantsToEatInput)
            {
                InputEater = null;
            }

            if (!Rectangle.Contains(inputHelper.MousePosition) && hideOverflow)
                inputStopped = true;

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
