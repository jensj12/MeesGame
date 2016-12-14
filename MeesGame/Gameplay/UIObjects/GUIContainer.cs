using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class GUIContainer : GUIObject
    {
        protected GUIObjectList<GUIObject> children;

        //Object that is using the input. This object will be asked every time HandleInput if it wants to keep eating the input
        private GUIObject inputEater = null;

        /// <summary>
        /// Contains GUI elements
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        public GUIContainer(Vector2? location, Vector2? dimensions, Color? backgroundColor = null) : base(location, dimensions, backgroundColor)
        {
            children = new GUIObjectList<GUIObject>();
        }

        public virtual void AddChild(GUIObject child)
        {
            child.Parent = this;
            children.Add(child);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //we check if the inputEater wants to keep eating our input
            //we can safely ask if the inputeater wants to use the input.
            if (InputEaten && !InputEater.WantsToEatInput)
            {
                InputEater = null;
            }

            if (!Visible) return;

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
            children.Clear();
        }

        /// <summary>
        /// Because in each GUI hierarchy we only want one object to use the input, we call the parent.
        /// </summary>
        public GUIObject InputEater
        {
            get
            {
                return Parent?.inputEater ?? inputEater;
            }
            set
            {
                if (Parent == null)
                    this.inputEater = value;
                else
                    Parent.inputEater = value;
            }
        }

        //This method is for the children. Every child might have a different anchor point in a sorted list.
        public virtual Vector2 GetChildAnchorPoint(GUIObject GUIObject)
        {
            return Vector2.Zero;
        }

        public GUIObjectList<GUIObject> Children
        {
            get { return children; }
        }

        public Boolean InputEaten
        {
            get { return InputEater != null; }
        }
    }
}
