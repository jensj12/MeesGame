using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class InputBox : Textbox
    {
        private bool selected;

        private bool textInput;

        private bool numberInput;

        public InputBox(Location location, Dimensions dimensions, string text, bool textInput = true, bool numberInput = true, string spritefont = null) : base(location, dimensions, text, spritefont)
        {
            this.textInput = textInput;
            this.numberInput = numberInput;
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
                    if ((s >= 'A' && s <= 'Z') || s == ' ' && textInput == true)
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

            set
            {
                base.InternalText = value;
            }
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
            //simple way to simulate an animated blinking ticker blinks every half a second.
            if(selected && gameTime.TotalGameTime.Milliseconds / 500 % 2 == 1)
            {
                Vector2 measuredCharacterDimensions = spriteFont.MeasureString(" ");
                spriteBatch.Draw(Utility.SolidWhiteTexture, new Rectangle((int)(anchorPoint.X + CurrentDimensions.X - measuredCharacterDimensions.X), (int)anchorPoint.Y, (int)measuredCharacterDimensions.X, (int)measuredCharacterDimensions.Y), Color.White);
            }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value;
                PermanentInvalid = value;
            }
        }

        public bool TextInput
        {
            get { return textInput; }
            set { textInput = value; }
        }

        public bool NumberInput
        {
            get { return numberInput; }
            set { numberInput = value; }
        }
    }
}
