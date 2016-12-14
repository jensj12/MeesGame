using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIObject : IGameLoopObject
    {
        /// <summary>
        /// location = relative location to the parent of the object
        /// dimensions = size of the element, used for input and for rendering when overflow is hidden
        /// parent = the parent of the object
        /// renderTarget = the texture of the ui element, so that we can render it when it is updated and disposed when not needed anymore
        /// invalidated = determines if the rendertarget needs updating
        /// </summary>
        private Vector2 relativeLocation;
        private Vector2 dimensions;
        protected UIContainer parent;
        protected RenderTarget2D renderTarget;
        protected bool invalidated = true;
        protected TextureRenderer textureRenderer;
        
        ///because the input is eaten after an element uses it we keep track of whether the elements
        ///have received an input and allow them to act accordingly in the update method
        private bool hovering;
        private bool mouseDown;
        private bool clicked;

        public bool Hovering
        {
            get { return hovering; }
        }

        public bool MouseDown
        {
            get { return mouseDown; }
        }

        public bool Clicked
        {
            get { return clicked; }
        }

        public bool Invalidate
        {
            get { return invalidated; }
            set {invalidated = value;
                if(parent != null)
                    parent.Invalidate = value;
            }
        }

        public virtual Vector2 AbsoluteLocation
        {
            get
            {
                if (parent != null)
                {
                    return RelativeLocation + parent.AbsoluteLocation;
                }

                return RelativeLocation;
            }
        }

        ///location relative to the location of its parent 
        public virtual Vector2 RelativeLocation
        {
            get
            {
                if (parent != null)
                    return relativeLocation + parent.GetChildAnchorPoint(this);
                return relativeLocation;
            }
        }

        public virtual Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        /// <summary>
        /// Gives the absolute location of the rectangle compared to the origin of the screen, 0,0
        /// useful for input checking
        /// </summary>
        public Rectangle AbsoluteRectangle
        {
            get { return new Rectangle(AbsoluteLocation.ToPoint(), Dimensions.ToPoint()); }
        }

        /// <summary>
        /// Gives the position relative to the parent
        /// Useful for drawing the texture we generate of the object
        /// </summary>
        public Rectangle RelativeRectangle
        {
            get { return new Rectangle(RelativeLocation.ToPoint(), Dimensions.ToPoint()); }
        }

        /// <summary>
        /// Gives the object with the location 0
        /// Useful for rendering the texture
        /// </summary>
        public Rectangle OriginLocationRectangle
        {
            get { return new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y); }
        }

        public UIContainer Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">location relative to the parent</param>
        /// <param name="dimensions">size of the object</param>
        /// <param name="parent">parent of the object</param>
        public UIObject(Vector2 location, Vector2 dimensions, UIContainer parent)
        {
            this.relativeLocation = location;
            this.dimensions = dimensions;
            this.parent = parent;
            this.textureRenderer = new TextureRenderer();
        }


        /// <summary>
        /// We read the input to determine if the ui element is being pointed at
        /// </summary>
        /// <param name="inputHelper"></param>
        public virtual void HandleInput(InputHelper inputHelper)
        {
            hovering = false;
            mouseDown = false;
            clicked = false;

            if (parent == null || !parent.InputEaten)
            {
                if (AbsoluteRectangle.Contains(inputHelper.MousePosition))
                {
                    hovering = true;
                    if(parent != null)
                        parent.InputEater = this;
                    if (inputHelper.MouseLeftButtonDown())
                    {
                        mouseDown = true;
                    }
                    if (inputHelper.MouseLeftButtonPressed())
                    {
                        clicked = true;
                    }
                }
            }
        }


        ///allows a component to use the input in the UI until it doesn't need the input anymore. If we wouldn't use this method, dragging any element
        ///would result in the input being registered for every element. Multiple buttons hovering at the same time for example
        public virtual bool WantsToEatInput
        {
            get {
                this.Invalidate = true;
                return false; }
        }

        /// <summary>
        /// Ui elements are able to override this at will, for example if the ui changes over time
        /// </summary>
        /// <param name="gameTime">current time</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// draws the UIel
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Invalidate == true)
            {
                renderTarget?.Dispose();
                textureRenderer.Render(gameTime, spriteBatch.GraphicsDevice, DrawTask, dimensions, out renderTarget);
                invalidated = false;
            }
            spriteBatch.Draw(renderTarget, RelativeRectangle, Color.White);
        }

        /// <summary>
        /// the draw task is basically the draw call, but with the added bonus of allowing the class to makeup the spritebatch using a scissor rectange for example
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="spriteBatch">a spritebatch to draw in</param>
        public abstract void DrawTask(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Reset() { }
    }
}
