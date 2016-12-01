using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIObject : IGameLoopObject
    {
        protected Vector2 location;
        protected Vector2 dimensions;
        protected UIObject parent;
        protected UIObjectList<UIObject> children;
        protected bool hideOverflow;
        protected TextureGenerator textureGenerator;
        protected RenderTarget2D renderTarget;
        //because the input is eaten after an element uses it we keep track of wether the elements
        //have received an input and allow them to act accordingly in the update method
        protected bool receivedInput;
        protected bool hovering;

        public UIObject(Vector2 location, Vector2 dimensions, UIObject parent, bool hideOverflow = false)
        {
            this.location = location;
            this.dimensions = dimensions;
            this.parent = parent;
            this.hideOverflow = hideOverflow;
            children = new UIObjectList<UIObject>();
        }

        public virtual Vector2 Location
        {
            get
            {
                if (parent != null)
                {
                    return location + parent.Location;
                }
                return location;
            }
            set { location = value; }
        }

        public virtual Vector2 RelativeLocation
        {
            get { return location; }
        }

        public bool Hovering
        {
            get { return hovering; }
        }

        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        //the rectangle gives the absolute location and dimensions of the uiobject
        public Rectangle Rectangle
        {
            get { return new Rectangle(Location.ToPoint(), Dimensions.ToPoint()); }
        }

        public UIObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public UIObjectList<UIObject> Children
        {
            get { return children; }
        }

        public bool HideOverflow
        {
            get { return hideOverflow; }
            set { hideOverflow = value; }
        }

        public virtual void HandleInput(InputHelper inputHelper)
        {
            receivedInput = true;
            if (Rectangle.Contains(inputHelper.MousePosition))
                hovering = true;
            else
                hovering = false;
            if (hideOverflow)
            {
                if (Rectangle.Contains(inputHelper.MousePosition))
                    children.HandleInput(inputHelper);
            }
            else
                children.HandleInput(inputHelper);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!receivedInput)
                hovering = false;
            receivedInput = false;
            children.Update(gameTime);

        }

        public virtual void DrawSelf(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (hideOverflow)
            {
                if (textureGenerator == null)
                {
                    textureGenerator = new TextureGenerator(spriteBatch.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
                }

                Vector2 myLocation = location;
                location = Vector2.Zero;
                renderTarget = textureGenerator.Render(gameTime, RenderTask);
                location = myLocation;
                spriteBatch.Draw(renderTarget, Rectangle, Color.White);
            }
            else
            {
                RenderTask(gameTime, spriteBatch);
            }
        }

        private void RenderTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawSelf(gameTime, spriteBatch);
            children.Draw(gameTime, spriteBatch);
        }

        public virtual void Reset()
        {
            children.Reset();
        }
    }
}
