using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MeesGame
{
    public class UIContainer : UIObject
    {
        protected UIObjectList<UIObject> children;

        //Object that is using the input. This object will be asked every time HandleInput if it wants to keep eating the input
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            if (Parent == null)
            {
                TextureRenderer.CreateMSpriteBatch(spriteBatch.GraphicsDevice);
                RenderTexture(gameTime, spriteBatch);
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
            }

            spriteBatch.Draw(renderTarget, RelativeRectangle, Color.White);
        }

        public override void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Invalidate)
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