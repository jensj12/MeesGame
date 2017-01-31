using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class InputBox : Textbox
    {
        public delegate void OnTextChangedEventhandler(InputBox inputBox);
        public event OnTextChangedEventhandler TextChanged;

        /// <summary>
        /// If the inputbox should use the input
        /// </summary>
        private bool selected;

        /// <summary>
        /// If letters are used as input
        /// </summary>
        private bool textInput;

        /// <summary>
        /// If numbers are used as input
        /// </summary>
        private bool numberInput;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="text">Text initially displayed before receiving input.</param>
        /// <param name="textInput">If letters should be used as input.</param>
        /// <param name="numberInput">If numbers should be used as input.</param>
        /// <param name="spritefont">Font with which the text should be rendered.</param>
        public InputBox(Location location, Dimensions dimensions, string text, bool textInput = true, bool numberInput = true, string spritefont = null) : base(location, dimensions, text, spritefont, Color.Black)
        {
            this.textInput = textInput;
            this.numberInput = numberInput;

            AddConstantComponent(new Background(Utility.SolidWhiteTexture, Color.White));
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (!MouseClicked && inputHelper.MouseLeftButtonPressed())
                Selected = false;
            if (MouseClicked)
                Selected = true;
            if (Selected)
            {
                List<Keys> pressedKeys = inputHelper.PressedKeys();
                //this is a very simple method to convert the input to actual letters. long press to repeat is not implemented.
                if (pressedKeys.Count == 1)
                {
                    if (pressedKeys[0] == Keys.Back && Text.Length > 0)
                        Text = Text.Remove(Text.Length - 1);
                    char s = (char)(int)pressedKeys[0];
                    if (((s >= 'A' && s <= 'Z') || s == ' ') && textInput == true)
                        Text += (char)(s == ' ' ? s : s + 32);
                    if (s >= '0' && s <= '9' && numberInput == true)
                        Text += s;
                }
            }
        }

        protected override string InternalText
        {
            get
            {
                return base.InternalText + " ";
            }
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            //simple way to simulate an animated blinking ticker blinks every half a second.
            if (selected && gameTime.TotalGameTime.Milliseconds / 500 % 2 == 1)
            {
                Vector2 measuredCharacterDimensions = spriteFont.MeasureString(" ");
                spriteBatch.Draw(Utility.SolidWhiteTexture, new Rectangle((int)(anchorPoint.X + MeasuredDimensions().X - measuredCharacterDimensions.X), (int)anchorPoint.Y, (int)measuredCharacterDimensions.X, (int)measuredCharacterDimensions.Y), textColor);
            }
        }

        public bool Selected
        {
            get { return selected; }
            set
            {
                if(selected != value)
                {
                    selected = value;
                    PermanentInvalid = value;
                }
            }
        }

        public bool TextInput
        {
            get { return textInput; }
            set
            {
                if (textInput != value)
                {
                    textInput = value;
                    TextChanged?.Invoke(this);
                }
            }
        }

        public bool NumberInput
        {
            get { return numberInput; }
            set { numberInput = value; }
        }
    }
}
