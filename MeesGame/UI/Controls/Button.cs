using MeesGame.UI;
using Microsoft.Xna.Framework.Audio;

namespace MeesGame
{
    public class Button : UIComponent
    {
        /// <summary>
        /// Indicates if the button is selected, for example when choosing a file in the fileExplorer.
        /// </summary>
        protected bool selected = false;

        /// <summary>
        /// The overlays over the background shown when hovering over the button.
        /// </summary>
        private UISpriteSheet hoveringOverlay;

        /// <summary>
        /// The overlays shown when the button is selected.
        /// </summary>
        private UISpriteSheet selectedOverlay;

        /// <summary>
        /// The textbox inside the Button.
        /// </summary>
        private Textbox textBox;

        /// <summary>
        /// Creates a button.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions">When this parameter is set to null, automatically set the Dimensions to inhertDimesions(this.Textbox)</param>
        /// <param name="text">The text displayed inside the button.</param>
        /// <param name="onClick">Method called when the button is clicked.</param>
        /// <param name="backgroundNames">Resource names of the used backgrounds.</param>
        /// <param name="hoverOverlayNames">Resource names of the used overlays when the button is being hovered.</param>
        /// <param name="selectedOverlayNames">Resource names of the used overlays when the button is selected.</param>
        /// <param name="textFont"></param>
        public Button(Location location, Dimensions dimensions, string text, string[] hoverOverlayNames = null, string[] selectedOverlayNames = null, OnClickEventHandler onClick = null, string textFont = null) : base(location, dimensions)
        {
            hoveringOverlay = new UISpriteSheet(SimpleLocation.Zero, InheritDimensions.All);

            selectedOverlay = new UISpriteSheet(SimpleLocation.Zero, InheritDimensions.All);

            textBox = new Textbox(SimpleLocation.Zero, InheritDimensions.All, text);

            if (dimensions == null)
                Dimensions = new MeasuredDimensions(textBox);
            AddComponent(textBox);

            Click += (UIComponent component) =>
            {
                GameEnvironment.AssetManager.PlaySound(DefaultUIValues.Default.ButtonSound);
            };

            if (onClick != null)
                Click += onClick;

            hoveringOverlay.AddSpritesheets(hoverOverlayNames ?? new string[] { DefaultUIValues.Default.DefaultButtonHoverBackground });
            hoveringOverlay.Visible = false;

            selectedOverlay.AddSpritesheets(selectedOverlayNames ?? new string[] { DefaultUIValues.Default.DefaultButtonSelectedBackground });
            selectedOverlay.Visible = false;

            AddComponent(hoveringOverlay);
            AddComponent(selectedOverlay);
        }

        /// <summary>
        /// Changes the text of the button.
        /// </summary>
        /// <param name="text"></param>
        public string Text
        {
            get { return Textbox.Text; }
            set
            {
                textBox.Text = value;
            }
        }

        /// <summary>
        /// Matches the button's dimensions to the measured dimensions of the string used as text.
        /// Only matches to the current dimensions of the string, for automatic dimensions matching when the text changes set the Dimensions property to MeasuredDimensions(Button.TextBox).
        /// </summary>
        public void MatchDimensionsToText()
        {
            Dimensions = new SimpleDimensions((int)textBox.MeasuredDimensions().X, (int)textBox.MeasuredDimensions().Y);
        }

        /// <summary>
        /// The textbox inside the button that displays the text.
        /// </summary>
        public Textbox Textbox
        {
            get { return textBox; }
        }

        /// <summary>
        /// Updates the hovering-backgrounds when the mouseHovering property changes.
        /// </summary>
        protected override void InputPropertyChanged()
        {
            hoveringOverlay.Visible = MouseHovering;
        }

        /// <summary>
        /// If the button is in the Selected state.
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    selectedOverlay.Visible = value;
                }
            }
        }
    }
}
