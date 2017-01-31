using MeesGame.UI;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    class SpriteSheetButton : Button
    {
        /// <summary>
        /// The background of the Button.
        /// </summary>
        private UISpriteSheet background;

        public SpriteSheetButton(Location location, Dimensions dimensions, string text, OnClickEventHandler onClick = null, int? edgeThickness = null, string[] backgroundNames = null, Color? hoverColor = null, string textFont = null, bool tiled = true) : base(location, dimensions, text, onClick, edgeThickness, hoverColor, textFont)
        {
            background = new UISpriteSheet(SimpleLocation.Zero, InheritDimensions.All, tiled: tiled);
            background.Parent = this;
            background.AddSpritesheets(backgroundNames ?? new string[] { DefaultUIValues.Default.DefaultButtonBackground });
            ConstantComponents.Insert(0, background);
        }
    }
}
