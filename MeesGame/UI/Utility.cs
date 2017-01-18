using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    static class Utility
    {
        /// <summary>
        /// Texture that holds a solid white color. It can be used to draw solid backgrounds
        /// </summary>
        private static Texture2D solidWhiteTexture;

        /// <summary>
        /// A 1x1 texture containing a White color. It can be used to color a surface using only a Color property
        /// in the spritebatch.draw(texture, rectangle, color) method.
        /// </summary>
        public static Texture2D SolidWhiteTexture
        {
            get
            {
                if (solidWhiteTexture == null)
                {
                    Color[] colordata = new Color[1];
                    colordata[0] = Color.White;
                    solidWhiteTexture = new Texture2D(GameEnvironment.Instance.GraphicsDevice, 1, 1);
                    solidWhiteTexture.SetData(colordata);
                }
                return solidWhiteTexture;
            }
        }

        /// <summary>
        /// Converts a System.Drawing.Color to a Microsoft.Xna.Framework.Color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color DrawingColorToXNAColor(System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns a transparent textures with white edges.
        /// </summary>
        /// <param name="width">Width of the desired texture.</param>
        /// <param name="height">Height of the desired texture.</param>
        /// <param name="thickness">Thickness of the edge.</param>
        /// <returns></returns>
        public static Texture2D EdgeTexture(int width, int height, int thickness)
        {
            Color[] colorData = new Color[width * height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    colorData[x + y * width] = (x < thickness || y < thickness || x > width - thickness || y > height - thickness) ? Color.White : Color.Transparent;
            Texture2D edgeTexture = new Texture2D(GameEnvironment.Instance.GraphicsDevice, width, height);
            edgeTexture.SetData(colorData);
            return edgeTexture;
        }
    }
}
