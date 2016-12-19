using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIObject : IGameLoopObject
    {
        public delegate void ClickEventHandler(UIObject uiObject);

        public event ClickEventHandler OnClick;

        /// <summary>
        /// Texture that holds a solid white color. It can be used to draw solid backgrounds
        /// </summary>
        private static Texture2D solidWhiteTexture;

        /// <summary>
        /// location = relative location to the parent of the object
        /// dimensions = size of the element, used for input and for rendering when overflow is hidden
        /// parent = the parent of the object
        /// renderTarget = the texture of the UI element, so that we can render it when it is updated and disposed when not needed anymore
        /// invalidated = determines if the rendertarget needs updating
        /// </summary>
        private Vector2 relativeLocation;

        private Vector2 dimensions;
        private UIContainer parent;
        private bool visible = true;
        protected Color? BackgroundColor;
        protected RenderTarget2D renderTarget;
        protected bool needsToBeInvalidated = true;
        protected TextureRenderer textureRenderer;

        ///because the input is eaten after an element uses it we keep track of whether the elements
        ///have received an input and allow them to act accordingly in the update method
        private bool hovering;

        private bool mouseDown;
        private bool clicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="location">location relative to the parent</param>
        /// <param name="dimensions">size of the object</param>
        /// <param name="parent">parent of the object</param>
        public UIObject(Vector2? location = null, Vector2? dimensions = null, Color? backgroundColor = null)
        {
            this.relativeLocation = location ?? Vector2.Zero;
            //dimensions is set to 1,1 because our graphics device can't compute a size of 0,0
            this.dimensions = dimensions ?? new Vector2(1);
            this.BackgroundColor = backgroundColor;
            this.textureRenderer = new TextureRenderer();
        }

        public static Texture2D SolidWhiteTexture(GraphicsDevice device)
        {
            if (solidWhiteTexture == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = Color.White;
                solidWhiteTexture = new Texture2D(device, 1, 1);
                solidWhiteTexture.SetData(colordata);
            }
            return solidWhiteTexture;
        }

        ///allows a component to use the input in the UI until it doesn't need the input anymore. If we wouldn't use this method, dragging any element
        ///would result in the input being registered for every element. Multiple buttons hovering at the same time for example
        public virtual bool WantsToEatInput
        {
            get { return false; }
        }

        /// <summary>
        /// We read the input to determine if the UI element is being pointed at
        /// </summary>
        /// <param name="inputHelper"></param>
        public virtual void HandleInput(InputHelper inputHelper)
        {
            if (!visible) return;
            hovering = false;
            mouseDown = false;
            clicked = false;

            if (parent == null || !parent.InputEaten)
            {
                if (AbsoluteRectangle.Contains(inputHelper.MousePosition))
                {
                    hovering = true;
                    if (parent != null)
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

                if (Clicked)
                {
                    InvokeClickEvent();
                }
            }
        }

        public void InvokeClickEvent()
        {
            OnClick?.Invoke(this);
            parent?.InvokeClickEvent();
        }

        /// <summary>
        /// UI elements are able to override this at will, for example if the UI changes over time
        /// </summary>
        /// <param name="gameTime">current time</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// draws the UI
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible) return;

            if (BackgroundColor != null)
                spriteBatch.Draw(SolidWhiteTexture(spriteBatch.GraphicsDevice), RelativeRectangle, (Color)BackgroundColor);
            if (Invalidate == true)
            {
                renderTarget?.Dispose();
                textureRenderer.Render(gameTime, spriteBatch.GraphicsDevice, DrawTask, dimensions, out renderTarget);
                Invalidate = false;
            }
            spriteBatch.Draw(renderTarget, RelativeRectangle, Color.White);
        }

        /// <summary>
        /// the draw task is basically the draw call, but with the added bonus of allowing the class to makeup the spritebatch using a scissor rectange for example
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="spriteBatch">a spritebatch to draw in</param>
        public abstract void DrawTask(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Reset()
        {
        }

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

        public virtual bool Invalidate
        {
            get { return needsToBeInvalidated; }
            set
            {
                needsToBeInvalidated = value;
                if (parent != null)
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
            set
            {
                relativeLocation = value;
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

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                Invalidate = true;
            }
        }
    }
}