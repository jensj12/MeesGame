﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class UIComponent
    {
        public delegate void OnClickEventHandler(UIComponent uiObject);

        /// <summary>
        /// When the UIComponent or one of its components is clicked.
        /// </summary>
        public event OnClickEventHandler Click;

        /// <summary>
        /// When a child of the UIComponent is clicked.
        /// </summary>
        public event OnClickEventHandler ChildClick;

        /// <summary>
        /// SpriteBatch without additional parameters in the draw method to avoid graphical glitches.
        /// </skummary>
        private static SpriteBatch uiSpriteBatch;

        /// <summary>
        /// If spriteBactch.begin(); has been called.
        /// </summary>
        private static bool uiSpriteBatchBegun = false;

        /// <summary>
        /// Location relative to the parent CurrentLocation of the parent excluding a child-dependent anchorpoint (for example in a sortedList)
        /// </summary>
        private Location location;

        /// <summary>
        /// Location cached at the beginning of the drawing method,
        /// </summary>
        private Vector2? cachedLocation;

        /// <summary>
        /// Size of the UIComponent.
        /// </summary>
        private Dimensions dimensions;

        /// <summary>
        /// Location cached at the beginning of the drawing method,
        /// </summary>
        private Point? cachedDimensions;

        /// <summary>
        /// Components that make up the object, for example a textbox and a background for a button.
        /// </summary>
        private ComponentList<UIComponent> constantComponents;

        /// <summary>
        /// Components that can be added to the component. Gets cleared on Reset(); and are drawn after the components are drawn.
        /// </summary>
        protected ComponentList<UIComponent> children;

        /// <summary>
        /// Usually only used in the highest parent of the UI structure. Specifies which object is using the input
        /// Required to prevent multiple objects inside the UI from using the input when it is already being used.
        /// If a component uses the input, than that component is the only component that should use the input until that same component specifies otherwise.
        /// </summary>
        private UIComponent inputUser = null;

        private UIComponent parent;

        /// <summary>
        /// If the component should be rendered. Equivalent to setting the Dimensions to Vector2.Zero, but without overwriting the Dimensions property.
        /// </summary>
        private bool visible = true;

        /// <summary>
        /// If the component should ever invalidate itself. If true, doesn't render to a texture.
        /// </summary>
        private bool permanentInvalid = false;

        /// <summary>
        /// A cached Texture of the component. Has to be updated every time the component is invalidated, unless the component is rendering directly (PermanentInvalid = true).
        /// </summary>
        protected RenderTarget2D selfTexture;

        private bool invalid = true;

        /// <summary>
        /// If the mouse is hovering over the component.
        /// </summary>
        private bool mouseHovering;

        /// <summary>
        /// If the left-mousebutton is down and hovering over the component.
        /// </summary>
        private bool mouseDown;

        /// <summary>
        /// If the mouse has clicked on the component.
        /// </summary>
        private bool mouseClicked;

        /// <param name="location">Location relative to the CurrentLocation of parent, automatically set to SimpleLocation.Zero if null.</param>
        /// <param name="dimensions">Size of the object.</param>
        public UIComponent(Location location, Dimensions dimensions)
        {
            this.location = location ?? SimpleLocation.Zero;
            this.dimensions = dimensions ?? InheritDimensions.All;

            children = new ComponentList<UIComponent>();
            constantComponents = new ComponentList<UIComponent>();
        }

        /// <summary>
        /// Adds a component at the end of the components list. For more direct control, Use the Components property.
        /// </summary>
        /// <param name="component"></param>
        public void AddConstantComponent(UIComponent component)
        {
            constantComponents.Add(component);
            component.Parent = this;
            UpdatePermanentInvalidProperty();
        }

        /// <summary>
        /// Adds a component at the end of the children list. For more direct control, see the Children property.
        /// </summary>
        /// <param name="component"></param>
        public virtual void AddChild(UIComponent child)
        {
            children.Add(child);
            child.Parent = this;
            child.UpdatePermanentInvalidProperty();
            child.Click += ChildClick;
        }

        /// <summary>
        /// Refreshes the bounds of the UIComponent and it's children and it's component's bounds.
        /// </summary>
        public virtual void RefreshCachedBounds()
        {
            Point newDimensions = CurrentDimensions;
            if (newDimensions != cachedDimensions)
            {
                Invalidate();
                cachedDimensions = newDimensions;
            }
            cachedLocation = CurrentRelativeLocation;
            children.RefreshCachedBounds();
            constantComponents.RefreshCachedBounds();
        }

        /// <summary>
        /// Requests the current inputUser if it still requires the Input. If it doesn't anymore, the inputUser can be released and other components will be able to use the input.
        /// </summary>
        public virtual bool WantsToUseInput
        {
            get { return MouseClicked; }
        }

        /// <summary>
        /// Reads the input and passes it over to the children and components.
        /// </summary>
        /// <param name="inputHelper"></param>
        public virtual void HandleInput(InputHelper inputHelper)
        {
            if (InputUser == this && !WantsToUseInput)
            {
                InputUser = null;
            }

            children.HandleInput(inputHelper);

            constantComponents.HandleInput(inputHelper);

            if (!Visible || !CanUseInput)
            {
                MouseHovering = MouseDown = MouseClicked = false;
                return;
            }

            //unnecessary changes to the input properties is prevented to avoid repeatedly calling the InputPropertieChanged() method.
            InterpretInput(inputHelper);
        }

        protected virtual void InterpretInput(InputHelper inputHelper)
        {
            bool mouseHovering = false;
            bool mouseDown = false;
            bool mouseClicked = false;

            if (AbsoluteRectangle.Contains(inputHelper.MousePosition))
            {
                mouseHovering = true;
                if (inputHelper.MouseLeftButtonDown())
                    mouseDown = true;
                if (inputHelper.MouseLeftButtonPressed())
                {
                    mouseClicked = true;
                    InputUser = this;
                    InvokeClickEvent();
                }
            }

            MouseHovering = mouseHovering;
            MouseDown = mouseDown;
            MouseClicked = mouseClicked;
        }

        public void InvokeClickEvent(UIComponent component = null)
        {
            Click?.Invoke(this);
        }

        /// <summary>
        /// Updates the children, the components and itself.
        /// </summary>
        /// <param name="gameTime">current time</param>
        public virtual void Update(GameTime gameTime)
        {
            children.Update(gameTime);
            constantComponents.Update(gameTime);
        }

        public static void BeginUISpriteBatch(SpriteBatch spriteBatch)
        {
            uiSpriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            uiSpriteBatchBegun = true;
        }

        /// <summary>
        /// Draws the UIComponent to the SpriteBatch.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2? anchorPoint = null)
        {
            if (!Visible) return;

            //Starts the uiSpritebatch if it hadn't been started already and draws the component through a texture to the screen to prevent graphical glitches.
            if (!uiSpriteBatchBegun)
            {
                RefreshCachedBounds();
                if (uiSpriteBatch == null)
                {
                    uiSpriteBatch = new SpriteBatch(spriteBatch.GraphicsDevice);
                }
                BeginUISpriteBatch(spriteBatch);

                RenderTargetBinding[] renderTargets = spriteBatch.GraphicsDevice.GetRenderTargets();
                RenderTexture(gameTime, uiSpriteBatch);
                spriteBatch.GraphicsDevice.SetRenderTargets(renderTargets);
                uiSpriteBatch.End();

                uiSpriteBatchBegun = false;
                spriteBatch.Draw(selfTexture, RelativeRectangle, Color.White);
            }
            else if (PermanentInvalid)
            {
                spriteBatch.End();

                spriteBatch.Begin(rasterizerState: new RasterizerState() { ScissorTestEnable = true }, samplerState: SamplerState.LinearWrap);

                spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((CachedRelativeLocation + (anchorPoint ?? Vector2.Zero)).ToPoint(), CachedDimensions);

                DrawTask(gameTime, spriteBatch, CachedRelativeLocation + (anchorPoint ?? Vector2.Zero));

                spriteBatch.End();

                BeginUISpriteBatch(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(selfTexture, new Rectangle((CachedRelativeLocation + (anchorPoint ?? Vector2.Zero)).ToPoint(), CachedDimensions), Color.White);
            }
        }

        /// <summary>
        /// Renders the UIComponent's texture if necessary.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void RenderTexture(GameTime gameTime, SpriteBatch spriteBatch)
        {
            constantComponents.RenderTexture(gameTime, spriteBatch);
            children.RenderTexture(gameTime, spriteBatch);

            if (!Visible || (PermanentInvalid && Parent != null)) return;

            if (Invalid)
            {
                if (selfTexture == null || selfTexture.IsDisposed || selfTexture.Bounds.Size != CachedDimensions)
                {
                    Invalidate();
                    if (selfTexture != null && selfTexture.Bounds.Size != CachedDimensions)
                        Dispose();
                    selfTexture = new RenderTarget2D(spriteBatch.GraphicsDevice, CachedDimensions.X, CachedDimensions.Y, false,
                        spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
                }
                TextureRenderer.Render(gameTime, DrawTask, spriteBatch, selfTexture);
                invalid = false;
            }
        }

        /// <summary>
        /// Draws the UIComponent to the location of the anchorpoint.
        /// </summary>
        public virtual void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            constantComponents.Draw(gameTime, spriteBatch, anchorPoint);
            children.Draw(gameTime, spriteBatch, anchorPoint);
        }

        /// <summary>
        /// Removes all children, resets itself and the Components and clears all textures.
        /// </summary>
        public virtual void Reset()
        {
            selfTexture?.Dispose();
            constantComponents.Reset();
            children.Clear();
        }

        /// <summary>
        /// Disposes all textures.
        /// </summary>
        public virtual void Dispose()
        {
            selfTexture?.Dispose();
            children.Dispose();
            constantComponents.Dispose();
        }

        public bool MouseHovering
        {
            get { return mouseHovering; }
            set
            {
                if (mouseHovering != value)
                {
                    mouseHovering = value;
                    InputPropertyChanged();
                }
            }
        }

        public bool MouseDown
        {
            get { return mouseDown; }
            set
            {
                if (mouseDown != value)
                {
                    mouseDown = value;
                    InputPropertyChanged();
                }
            }
        }

        public bool MouseClicked
        {
            get { return mouseClicked; }
            set
            {
                if (mouseClicked != value)
                {
                    mouseClicked = value;
                    InputPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Returns true if the selfTexture needs to be updated.
        /// </summary>
        protected virtual bool Invalid
        {
            get
            {
                return PermanentInvalid || invalid || (selfTexture?.IsDisposed ?? true);
            }
        }

        /// <summary>
        /// Gets and set the permanentinvalid property, and updates the parents permanentinvalidproperties when set.
        /// </summary>
        public virtual bool PermanentInvalid
        {
            get { return permanentInvalid; }
            set
            {
                if (permanentInvalid != value)
                {
                    if (value == false)
                        Invalidate();
                    permanentInvalid = value;
                    Parent?.UpdatePermanentInvalidProperty();
                }
            }
        }

        /// <summary>
        /// Updates the permanentInvalid property.
        /// </summary>
        public virtual void UpdatePermanentInvalidProperty()
        {
            foreach (UIComponent child in constantComponents)
                if (child.PermanentInvalid)
                {
                    PermanentInvalid = true;
                    return;
                }
            foreach (UIComponent child in children)
                if (child.PermanentInvalid)
                {
                    PermanentInvalid = true;
                    return;
                }

            PermanentInvalid = false;
            return;
        }

        /// <summary>
        /// Forces the object to redraw itself.
        /// </summary>
        public void Invalidate()
        {
            if (!invalid)
            {
                invalid = true;
                if (Parent != null)
                    Parent.Invalidate();
            }
        }

        /// <summary>
        /// Specifies the location of the object relative to the base of the UIComponent structure (the only object in the structure without a parent)
        /// </summary>
        public virtual Vector2 AbsoluteLocation
        {
            get
            {
                if (parent != null)
                {
                    return CachedRelativeLocation + parent.AbsoluteLocation;
                }

                return CachedRelativeLocation;
            }
        }

        /// <summary>
        /// Location relative to the location of its parent.
        /// </summary>
        public virtual Vector2 CachedRelativeLocation
        {
            get
            {
                if (cachedLocation == null)
                {
                    cachedLocation = CurrentRelativeLocation;
                }
                return (Vector2)cachedLocation;
            }
        }

        /// <summary>
        /// Dimensions of the UIObject.
        /// </summary>
        public virtual Point CachedDimensions
        {
            get
            {
                if (cachedDimensions == null)
                {
                    cachedDimensions = CurrentDimensions;
                }
                return (Point)cachedDimensions;
            }
        }

        /// <summary>
        /// Relative location as calculated by the Location property.
        /// </summary>
        public virtual Vector2 CurrentRelativeLocation
        {
            get
            {
                return location.ToVector2(this) + (Parent?.ChildAnchorPoint(this) ?? Vector2.Zero);
            }
        }

        /// <summary>
        /// Relative location as calculated by the Location property.
        /// </summary>
        public virtual Point CurrentDimensions
        {
            get
            {
                return dimensions.ToPoint(this);
            }
        }

        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
            }
        }

        public Dimensions Dimensions
        {
            get { return dimensions; }
            set
            {
                dimensions = value;
            }
        }

        /// <summary>
        /// Contains the absolute location of the rectangle compared to the origin of the screen (0,0)
        /// Useful for input checking
        /// </summary>
        public Rectangle AbsoluteRectangle
        {
            get { return new Rectangle(AbsoluteLocation.ToPoint(), CachedDimensions); }
        }

        /// <summary>
        /// Gives the position relative to the parent.
        /// Useful for drawing the object as a child of the parent.
        /// </summary>
        public Rectangle RelativeRectangle
        {
            get { return new Rectangle(CachedRelativeLocation.ToPoint(), CachedDimensions); }
        }

        /// <summary>
        /// Gives the object with the location Point.Zero.
        /// Useful for rendering the texture.
        /// </summary>
        public Rectangle PointZeroLocationRectangle
        {
            get { return new Rectangle(Point.Zero, CachedDimensions); }
        }

        /// <summary>
        /// Returns the location at which the child should draw itself.
        /// </summary>
        /// <param name="uiObject"></param>
        /// <returns></returns>
        public virtual Vector2 ChildAnchorPoint(UIComponent uiComponent)
        {
            return Vector2.Zero;
        }

        /// <summary>
        /// Returns the Component that is using the input at the moment.
        /// </summary>
        public UIComponent InputUser
        {
            get
            {
                return Parent?.InputUser ?? inputUser;
            }
            set
            {
                if (InputUser == null || value == null)
                    if (Parent == null)
                        inputUser = value;
                    else
                        Parent.InputUser = value;
            }
        }

        public UIComponent Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// If the input is already in use by another UIComponent
        /// </summary>
        public bool CanUseInput
        {
            get { return InputUser == null || inputUser == this || ContainsComponent(InputUser); }
        }

        public bool ContainsComponent(UIComponent component)
        {
            return constantComponents.Contains(component);
        }

        public ComponentList<UIComponent> Children
        {
            get { return children; }
        }

        public ComponentList<UIComponent> ConstantComponents
        {
            get { return constantComponents; }
        }

        protected virtual void InputPropertyChanged()
        {
        }

        /// <summary>
        /// If the component should be rendered. Equivalent to setting the Dimensions to Vector2.Zero, but without overwriting the Dimensions property.
        /// </summary>
        public bool Visible
        {
            get { return visible && Parent?.Visible != false && CachedDimensions != Point.Zero; }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    Invalidate();
                }
            }
        }
    }
}