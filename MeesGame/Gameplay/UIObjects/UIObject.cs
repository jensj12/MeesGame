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
        /// relativeLocation = relative location to the parent of the object
        /// dimensions = size of the element, used for input and for rendering
        /// renderTarget = the texture of the UI element, so that we can render it when it is updated and disposed when not needed anymore
        /// needsToBeInvalidated = determines if the rendertarget needs updating
        /// textureRenderer = used to render the texture for the UI
        /// </summary>
        private Vector2 relativeLocation;
        private Vector2 dimensions;
        private UIContainer parent;
        private bool visible = true;
        protected Color? backgroundColor;
        protected RenderTarget2D renderTarget;
        protected bool needsToBeInvalidated = true;

        ///because the input is eaten after an element uses it we keep track of whether the elements
        ///have received an input and allow them to act accordingly in the update method
        private bool hovering;

        private bool mouseDown;
        private bool clicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="location">location relative to the parent, equals Vector2.Zero if left empty</param>
        /// <param name="dimensions">size of the object</param>
        public UIObject(Vector2? location = null, Vector2? dimensions = null, Color? backgroundColor = null)
        {
            this.relativeLocation = location ?? Vector2.Zero;
            this.dimensions = dimensions ?? Vector2.Zero;
            this.backgroundColor = backgroundColor;
        }

        protected static Texture2D SolidWhiteTexture
        {
            get
            {
                if (solidWhiteTexture == null)
                {
                    Color[] colordata = new Color[1];
                    colordata[0] = Color.White;
                    solidWhiteTexture = new Texture2D(GameEnvironment.Instance.GraphicsDevice, 1, 1);
                    solidWhiteTexture.SetData(colordata);
                }
                return solidWhiteTexture;
            }
            set
            {
                solidWhiteTexture = value;
            }
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
        /// Draws to the spritebatch
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.Draw(renderTarget, RelativeRectangle, Color.White);
        }

        /// <summary>
        /// Draws to the UIObject's texture
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            if (Invalidate)
            {
                renderTarget?.Dispose();
                TextureRenderer.Render(gameTime, DrawTask, dimensions, out renderTarget);
                Invalidate = false;
            }
        }

        /// <summary>
        /// Draws to the renderTarget
        /// </summary>
        /// <param name="gameTime">the current gametime</param>
        /// <param name="spriteBatch">a spritebatch to draw in</param>
        public virtual void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (backgroundColor != null)
                spriteBatch.Draw(SolidWhiteTexture, OriginLocationRectangle, (Color)backgroundColor);
        }

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
                if (parent != null && value == true)
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
            get { return visible && Dimensions != Vector2.Zero; }
            set
            {
                visible = value;
                Invalidate = true;
            }
        }
    }
}