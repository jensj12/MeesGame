using MeesGame.UI;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class RadioButton : UIComponent
    {
        private const int distance = 20;

        private List<RadioButton> radioGroup;

        private Button button;

        private Textbox textbox;

        public RadioButton(Location location, string text = "", List<RadioButton> radioGroup = null, OnClickEventHandler onClick = null) : base(location, null) 
        {
            Dimensions = WrapperDimensions.All;

            RadioGroup = radioGroup;

            button = new Button(SimpleLocation.Zero, new SimpleDimensions(DefaultUIValues.Default.RadioButtonDimen, DefaultUIValues.Default.RadioButtonDimen), "");

            textbox = new Textbox(new SimpleLocation(DefaultUIValues.Default.RadioButtonDimen + distance, 0), null, text, DefaultUIValues.Default.DefaultEditorControlSpriteFont, Color.Black);

            AddConstantComponent(button);
            AddConstantComponent(textbox);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (MouseHovering)
            {
                button.MouseHovering = true;
            }
            if ((button.MouseClicked || MouseClicked) && !Selected && radioGroup != null)
            {
                //Make the click sound even if the button wasn't clicked.
                if(!button.MouseClicked)
                    Button.InvokeClickEvent(this);
                foreach(RadioButton radioButton in radioGroup)
                {
                    radioButton.Selected = false;
                }
                Selected = true;
            }
        }

        /// <summary>
        /// Returns the Button inside the radioButton.
        /// </summary>
        public Button Button
        {
            get { return button; }
        }

        public bool Selected
        {
            get { return button.Selected; }
            set { button.Selected = value; }
        }

        /// <summary>
        /// Contains the group of radiobuttons. Only one radiobutton in the group can be selected at any given time.
        /// </summary>
        private List<RadioButton> RadioGroup
        {
            get { return radioGroup; }
            set {
                radioGroup?.Remove(this);
                radioGroup = value;
                radioGroup.Add(this);
            }
        }
    }
}
