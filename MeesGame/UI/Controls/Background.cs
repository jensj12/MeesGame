using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    /// <summary>
    /// Texture that inherits its parent's dimensions.
    /// </summary>
    class Background : Texture
    {
        public Background(Texture2D texture, Color? color = default(Color?)) : base(new SimpleLocation(), new InheritDimensions(true, true), texture, color)
        {
        }
    }
}
