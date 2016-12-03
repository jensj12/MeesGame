using MeesGame.Gameplay.UIObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public abstract class UIObject : IGameLoopObject
    {
        protected Vector2 location;
        protected Vector2 dimensions;
        protected UIContainer parent;
        protected bool hideOverflow;
        protected TextureGenerator textureGenerator;
        protected RenderTarget2D renderTarget;
        //because the input is eaten after an element uses it we keep track of wether the elements
        //have received an input and allow them to act accordingly in the update method
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


        public UIObject(Vector2 location, Vector2 dimensions, UIContainer parent, bool hideOverflow = false)
        {
            this.location = location;
            this.dimensions = dimensions;
            this.parent = parent;
            this.hideOverflow = hideOverflow;
        }

        public virtual Vector2 Location
        {
            get
            {
                if (parent != null)
                {
                    return location + parent.GetChildAnchorPoint(this);
                }

                return location;
            }
        }

        public virtual Vector2 RelativeLocation
        {
            get { return location; }
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

        public UIContainer Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public bool HideOverflow
        {
            get { return hideOverflow; }
            set { hideOverflow = value; }
        }

        //if this 
        public virtual void HandleInput(InputHelper inputHelper)
        {
            hovering = false;
            mouseDown = false;
            clicked = false;

            if (parent == null || !parent.InputEaten)
            {
                if (Rectangle.Contains(inputHelper.MousePosition))
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


        //if a component such as the scroll bar wants to eat the input it can remain eaten
        public virtual bool WantsToEatInput
        {
            get { return false; }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (hideOverflow)
            {
                if (textureGenerator == null)
                {
                    textureGenerator = new TextureGenerator(spriteBatch.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
                }

                Vector2 myLocation = location;
                //if we draw an object in a seperate container to hide the overflow we need to make sure the object get's drawn that is 0,0 relative to the parent.
                //thus we temporarily move the parent to 0,0 so that it get's drawn there. For now no other way (perhaps with scissor rectangles in the future
                UIContainer myParent = parent;
                location = Vector2.Zero;
                renderTarget = textureGenerator.Render(gameTime, DrawTask);
                location = myLocation;
                parent = myParent;
                spriteBatch.Draw(renderTarget, Rectangle, Color.White);
            }
            else
            {
                DrawTask(gameTime, spriteBatch);
            }
        }

        // the way an object should draw itself. We have to make the object draw itself in a sepperate method from the draw call because
        // we need to do some prelimary work to help the object hide its overflow
        public abstract void DrawTask(GameTime gameTime, SpriteBatch spriteBatch);
        public virtual void Reset() { }
    }
}
