using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class TextureButton : Button
    {
        Background background;

        public TextureButton(Location location, Dimensions dimensions, string text, Texture2D texture = null, Color? color = null, OnClickEventHandler onClick = null, int? edgeThickness = null, Color? hoverColor = null, string textFont = null) : base(location, dimensions, text, onClick, edgeThickness, hoverColor, textFont)
        {
            background = new Background(texture, color);
            background.Parent = this;
            ConstantComponents.Insert(0, background);
        }

        public Color Color
        {
            get { return background.Color; }
        }
    }
}
