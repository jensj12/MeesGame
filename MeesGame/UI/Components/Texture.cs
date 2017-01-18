using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    /// <summary>
    /// Renders texture to the UIComponent
    /// </summary>
    class Texture : UIComponent
    {
        private Texture2D texture;
        private Color color;

        public Texture(Location location, Dimensions dimensions, Texture2D texture, Color? color = null) : base(location, dimensions)
        {
            this.texture = texture;
            this.color = color ?? Color.White;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            spriteBatch.Draw(texture, new Rectangle(anchorPoint.ToPoint(), CurrentDimensions), color);
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
        }

        public Texture2D CurrentTexture
        {
            get { return texture; }
            set
            {
                texture?.Dispose();
                texture = value;
            }
        }

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                Invalidate();
            }
        }
    }
}
