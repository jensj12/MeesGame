using MeesGame.UI;

namespace MeesGame
{
    class SpriteSheetButton : Button
    {
        /// <summary>
        /// The background of the Button.
        /// </summary>
        private UISpriteSheet background;

        public SpriteSheetButton(Location location, Dimensions dimensions, string text, OnClickEventHandler onClick = null, string[] backgroundNames = null, string[] hoverOverlayNames = null, string[] selectedOverlayNames = null, string textFont = null) : base(location, dimensions, text, hoverOverlayNames, selectedOverlayNames, onClick, textFont)
        {
            background = new UISpriteSheet(SimpleLocation.Zero, InheritDimensions.All);

            background.Parent = this;

            background.AddSpritesheets(backgroundNames ?? new string[] { DefaultUIValues.Default.DefaultButtonBackground });

            ConstantComponents.Insert(0, background);
        }
    }
}
