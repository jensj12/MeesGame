using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MeesGame.UI;

namespace MeesGame
{
    class Slider : UIComponent
    {
        public delegate void OnValueChangedHandler(int value);
        public event OnValueChangedHandler ValueChanged;

        private const int SLIDER_WIDTH = 40;
        private const int SLIDER_HEIGHT = 20;
        private const int SLIDER_BAR_HEIGHT = 10;

        /// <summary>
        /// Lower bound.
        /// </summary>
        private int min;

        /// <summary>
        /// Uper bound.
        /// </summary>
        private int max;

        /// <summary>
        /// Value the slider currently represents.
        /// </summary>
        private int value;

        /// <summary>
        /// If te slider is currently being dragged.
        /// </summary>
        private bool dragging = false;

        /// <summary>
        /// X value of the mouseposition when the dragging started
        /// </summary>
        private float startMouseX;

        /// <summary>
        /// The value when the dragging started.
        /// </summary>
        private int startValue;

        public Slider(Location location, Dimensions dimensions, int min = 0, int max = 100, int value = 100) : base(location, dimensions)
        {
            this.min = min;
            this.max = max;
            this.value = value;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            spriteBatch.Draw(Utility.SolidWhiteTexture, new Rectangle((CurrentSliderBarLocation + anchorPoint).ToPoint(), SliderBarDimensions), Utility.DrawingColorToXNAColor(DefaultUIValues.Default.SliderBarColor));
            spriteBatch.Draw(Utility.SolidWhiteTexture, new Rectangle((CurrentSliderLocation + anchorPoint).ToPoint(), SliderDimensions), Utility.DrawingColorToXNAColor(DefaultUIValues.Default.SliderColor));
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (MouseHovering)
            {
                if (new Rectangle(AbsoluteSliderLocation.ToPoint(), SliderDimensions).Contains(inputHelper.MousePosition) && !Dragging && MouseDown)
                {
                    Dragging = true;
                    startMouseX = inputHelper.MousePosition.X;
                    startValue = Value;
                }
            }
            if (Dragging)
            {
                if (!inputHelper.MouseLeftButtonDown())
                {
                    Dragging = false;
                    return;
                }

                value = startValue + (int)((inputHelper.MousePosition.X - startMouseX) * (max - min) / SliderBarDimensions.X);

                Value = MathHelper.Clamp(value, min, max);
            }
        }

        public override bool WantsToUseInput
        {
            get
            {
                return base.WantsToUseInput || Dragging;
            }
        }

        /// <summary>
        /// Value the slider currently represents.
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                ValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// If te slider is currently being dragged.
        /// </summary>
        private bool Dragging
        {
            get { return dragging; }
            set
            {
                PermanentInvalid = value;
                dragging = value;
            }
        }

        /// <summary>
        /// Location of the object that can be slid.
        /// </summary>
        private Vector2 CurrentSliderLocation
        {
            get
            {
                return new Vector2((SliderBarDimensions.X) * ((value - min) / (float)(max - min)), SLIDER_BAR_HEIGHT);
            }
        }

        /// <summary>
        /// Absolute location of the object that can be slid.
        /// </summary>
        private Vector2 AbsoluteSliderLocation
        {
            get
            {
                return CurrentSliderLocation + AbsoluteLocation;
            }
        }

        /// <summary>
        /// Dimensions of the objects that can be slid.
        /// </summary>
        private Point SliderDimensions
        {
            get
            {
                return new Point(SLIDER_WIDTH, SLIDER_HEIGHT);
            }
        }

        /// <summary>
        /// Bar along which the slider moves.
        /// </summary>
        private Vector2 CurrentSliderBarLocation
        {
            get
            {
                return new Vector2(SLIDER_WIDTH / 2, SLIDER_HEIGHT / 2 + SLIDER_BAR_HEIGHT / 2);
            }
        }

        /// <summary>
        /// Absolute position of the bar along which the slider moves.
        /// </summary>
        private Vector2 AbsoluteSliderBarLocation
        {
            get
            {
                return CurrentSliderLocation + AbsoluteLocation;
            }
        }

        /// <summary>
        /// Dimensions of the bar along which the slider moves
        /// </summary>
        private Point SliderBarDimensions
        {
            get
            {
                return new Point(CurrentDimensions.X - SLIDER_WIDTH, SLIDER_BAR_HEIGHT);
            }
        }
    }
}
