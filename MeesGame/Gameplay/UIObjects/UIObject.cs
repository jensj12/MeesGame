using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIObject : IGameLoopObject
    {
        public delegate void OnClickEventHandler(UIObject uiObject);

        /// <summary>
        /// When the UIObject is clicked this event is called
        /// </summary>
        public event OnClickEventHandler Click;

        /// <summary>
        /// Texture that holds a solid white color. It can be used to draw solid backgrounds
        /// </summary>
        private static Texture2D solidWhiteTexture;

        /// <summary>
        /// relativeLocation = relative location to the parent of the object
        /// </summary>
        private Vector2 relativeLocation;

        /// <summary>
        /// Size of the element, used for input and for rendering
        /// </summary>
        private Vector2 dimensions;

        private UIContainer parent;
        private bool visible = true;
        protected Color? backgroundColor;
        protected RenderTarget2D objectTexture;
        
        private bool mouseHovering;
        private bool mouseDown;
        private bool mouseClicked;
        
        /// <param name="location">location relative to the parent, equals Vector2.Zero if left empty</param>
        /// <param name="dimensions">size of the object</param>
        public UIObject(Vector2? location = null, Vector2? dimensions = null, Color? backgroundColor = null)
        {
            this.relativeLocation = location ?? Vector2.Zero;
            this.dimensions = dimensions ?? Vector2.Zero;
            this.backgroundColor = backgroundColor;
        }

        /// <summary>
        /// The SolidWhiteTexture is a white 1x1 texture. It can be used to color a surface using only a Color property
        /// in the spritebatch.draw(texture, rectangle, color) method.
        /// </summary>
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
        ///would result in the input being registered for every element the mouse hovers over in the meantime.
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
            mouseHovering = false;
            mouseDown = false;
            mouseClicked = false;

            if (parent == null || !parent.InputEaten)
            {
                if (AbsoluteRectangle.Contains(inputHelper.MousePosition))
                {
                    mouseHovering = true;
                    if (parent != null && Visible)
                        parent.InputEater = this;
                    if (inputHelper.MouseLeftButtonDown())
                    {
                        mouseDown = true;
                    }
                    if (inputHelper.MouseLeftButtonPressed())
                    {
                        mouseClicked = true;
                    }
                }

                if (Clicked)
                {
                    InvokeOnClickEvent();
                }
            }
        }

        /// <summary>
        /// invokes the OnClickEvent
        /// </summary>
        public void InvokeOnClickEvent()
        {
            Click?.Invoke(this);
        }

        /// <summary>
        /// Updates the UIElement
        /// </summary>
        /// <param name="gameTime">current time</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the UIElement to the SpriteBatch. Until ScissorRectangles works it always uses a texture (objectTexture)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.Draw(objectTexture, RelativeRectangle, Color.White);
        }

        /// <summary>
        /// renders the UIObject's texture
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            if (Invalid)
            {
                objectTexture?.Dispose();
                TextureRenderer.Render(gameTime, DrawTask, dimensions, out objectTexture);
            }
        }

        /// <summary>
        /// In the DrawTask method we draw the individual parts of the UIObject to the SpriteBatch, which is then
        /// put into a texture we can use to draw this UIObject until it is invalidated.
        /// </summary>
        public virtual void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (backgroundColor != null)
                spriteBatch.Draw(SolidWhiteTexture, OriginLocationRectangle, (Color)backgroundColor);
        }

        /// <summary>
        /// cleanly disposes unnecessary textures
        /// </summary>
        public virtual void Reset()
        {
            objectTexture?.Dispose();
        }

        public bool Hovering
        {
            get { return mouseHovering; }
        }

        public bool MouseDown
        {
            get { return mouseDown; }
        }

        public bool Clicked
        {
            get { return mouseClicked; }
        }

        /// <summary>
        /// returns false if the object doesn't contain a texture
        /// </summary>
        protected virtual bool Invalid
        {
            get
            {
                return objectTexture == null;
            }
        }

        /// <summary>
        /// Forces the object to redraw itself
        /// </summary>
        public void Invalidate()
        {
            {
                objectTexture?.Dispose();
                objectTexture = null;
                if (Parent != null)
                    Parent.Invalidate();
            }
        }

        /// <summary>
        /// Specifies the location of the object relative to the base of the UIObject structure (the only object in the structure without a parent)
        /// </summary>
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

        /// <summary>
        /// Location relative to the location of its parent
        /// get : returns the location relative to the distance from the parent's anchor point
        /// set : sets the location relative to the parents location EXCLUDING the anchor point
        /// </summary>
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
        /// Gives the absolute location of the rectangle compared to the origin of the screen (0,0)
        /// Useful for input checking
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
        /// When reading, it also checks if the dimensions aren't zero.
        /// </summary>
        public bool Visible
        {
            get { return visible && Parent?.Visible != false && Dimensions != Vector2.Zero; }
            set
            {
                visible = value;
                Invalidate();
            }
        }
    }
}
