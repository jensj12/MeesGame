using Microsoft.Xna.Framework.Input;
using System;
using System.Text.RegularExpressions;

namespace MeesGame
{
    class NumberInputBox : InputBox
    {
        public delegate void OnNumberChangedEventHandler(NumberInputBox numberInputBox);
        public event OnNumberChangedEventHandler NumberChanged;

        /// <summary>
        /// The current value the numberinputbox holds.
        /// </summary>
        int number;

        /// <summary>
        /// The minimal value the numberinputbox can hold
        /// </summary>
        int min;

        /// <summary>
        /// The maximal value the numberinputbox can hold
        /// </summary>
        int max;

        /// <summary>
        /// If the initial text has been removed
        /// </summary>
        bool initialTextRemoved = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="text"></param>
        /// <param name="startNumber"></param>
        /// <param name="min">Lowest number allowed to be put into the input.</param>
        /// <param name="max">Biggest number allowed to be put into the input.</param>
        /// <param name="spritefont"></param>
        public NumberInputBox(Location location, Dimensions dimensions, string text, int startNumber = 0, int min = 0, int max = 100, string spritefont = null) : base(location, dimensions, text, false, true, spritefont)
        {
            number = startNumber;
            this.min = min;
            this.max = max;
        }
        
        /// <summary>
        /// The number the numberinputbox holds.
        /// </summary>
        public int Number
        {
            get {
                if (Text == "")
                    return min;
                return number; }
            set {
                if(number == value)
                {
                    return;
                }
                if (value < min)
                    number = min;
                else if (value > max)
                    number = max;
                else
                {
                    number = value;
                    NumberChanged?.Invoke(this);    
                }
                Text = number.ToString();
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (Selected)
            {
                if (initialTextRemoved == false)
                {
                    Text = Number.ToString();
                    initialTextRemoved = true;
                }
                if (Regex.IsMatch(Text, @"^\d+$"))
                {
                    Number = int.Parse(Text);
                }
                if (inputHelper.KeyPressed(Keys.Up))
                {
                    Number++;
                }
                if (inputHelper.KeyPressed(Keys.Down))
                {
                    Number--;
                }
            }
        }
    }
}
