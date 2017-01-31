using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class Button : UIComponent
    {
        public event OnClickEventHandler SelectedChanged;

        /// <summary>
        /// Indicates if the button is selected, for example when choosing a file in the fileExplorer.
        /// </summary>
        protected bool selected = false;

        /// <summary>
        /// The overlays over the background shown when hovering over the button.
        /// </summary>
        private Texture hoveringOverlay;

        private EdgeTexture edgeTexture;

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
        /// <param name="hoverColor">Color that is drawn over the background of the Button.</param>
        /// <param name="textFont">Font the textbox uses.</param>
        public Button(Location location, Dimensions dimensions, string text, OnClickEventHandler onClick = null, int? edgeThickness = null, Color? hoverColor = null, string textFont = null) : base(location, dimensions)
        {
            hoveringOverlay = new Background(Utility.SolidWhiteTexture, hoverColor ?? Utility.DrawingColorToXNAColor(DefaultUIValues.Default.ButtonHoverColor));

            textBox = new Textbox(CenteredLocation.All, null, text);
            hoveringOverlay.Visible = false;

            edgeTexture = new EdgeTexture(edgeThickness, Utility.DrawingColorToXNAColor(DefaultUIValues.Default.ButtonEdgeColor));

            if (dimensions == null)
                Dimensions = new MeasuredDimensions(textBox, 2 * DefaultUIValues.Default.EdgeThickness, 2 * DefaultUIValues.Default.EdgeThickness);

            Click += (UIComponent component) =>
            {
                GameEnvironment.AssetManager.PlaySound(DefaultUIValues.Default.ButtonSound);
            };

            if (onClick != null)
                Click += onClick;

            AddConstantComponent(hoveringOverlay);
            AddConstantComponent(textBox);
            AddConstantComponent(edgeTexture);
        }

        /// <summary>
        /// Changes the text of the button.
        /// </summary>
        /// <param name="text"></param>
        public string Text
        {
            get { return textBox.Text; }
            set
            {
                textBox.Text = value;
            }
        }
        
        /// <summary>
        /// Updates the hovering-backgrounds when the mouseHovering property changes.
        /// </summary>
        protected override void InputPropertyChanged()
        {
            hoveringOverlay.Visible = MouseHovering;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
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
                    if (selected)
                        edgeTexture.EdgeColor = Utility.DrawingColorToXNAColor(DefaultUIValues.Default.DefaultButtonSelectedEdgeColor);
                    else
                        edgeTexture.EdgeColor = Utility.DrawingColorToXNAColor(DefaultUIValues.Default.ButtonEdgeColor);
                    SelectedChanged?.Invoke(this);
                    Invalidate();
                }
            }
        }
    }
}
