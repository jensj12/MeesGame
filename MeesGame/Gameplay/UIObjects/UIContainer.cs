using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MeesGame
{
    public class UIContainer : UIObject
    {
        public event OnClickEventHandler ChildClick;

        protected UIObjectList<UIObject> children;

        /// <summary>
        /// Only used in the parent of the UI structure. Specifies which object is using the input
        /// Required to prevent multiple objects inside the UI from using the input when it is already being used.
        /// </summary>
        private UIObject inputEater = null;

        /// <summary>
        /// Contains UI elements
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        public UIContainer(Vector2? location, Vector2? dimensions, Color? backgroundColor = null) : base(location, dimensions, backgroundColor)
        {
            children = new UIObjectList<UIObject>();
        }

        public virtual void AddChild(UIObject child)
        {
            child.Parent = this;
            children.Add(child);
            child.Click += OnChildClick;
        }

        protected virtual void OnChildClick(UIObject o)
        {
            ChildClick?.Invoke(o);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //we check if the inputEater wants to keep eating our input
            //we can safely ask if the inputeater wants to use the input.
            if (InputEater?.WantsToEatInput == false)
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            //If the container doesn't have a parent it is the base of the UI structure, and therefore
            //its RenderTexture method is never called. Once we finish rendering all textures we reset the rendertarget.
            if (Parent == null)
            {
                RenderTargetBinding[] renderTargets = spriteBatch.GraphicsDevice.GetRenderTargets();
                RenderTexture(gameTime, spriteBatch);
                spriteBatch.GraphicsDevice.SetRenderTargets(renderTargets);
            }

            spriteBatch.Draw(objectTexture, RelativeRectangle, Color.White);
        }

        public override void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Invalid)
            {
                children.RenderTexture(gameTime, spriteBatch);
                base.RenderTexture(gameTime, spriteBatch);
            }
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawTask(gameTime, spriteBatch);
            children.Draw(gameTime, spriteBatch);
        }

        public override void Reset()
        {
            children.Clear();
        }

        /// <summary>
        /// Because in each UI hierarchy we only want one object to use the input, we call the parent.
        /// </summary>
        public UIObject InputEater
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
    }
}
