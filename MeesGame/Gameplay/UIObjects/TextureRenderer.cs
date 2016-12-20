using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MeesGame
{
    public static class TextureRenderer
    {
        public delegate void RenderTask(GameTime gameTime, SpriteBatch spriteBatch);

        //SpriteBatche used for rendering our texture.
        private static SpriteBatch mSpriteBatch;

        /// <summary>
        /// Render the task to a texture
        /// </summary>
        /// <param name="gameTime">Time used for the rendertask</param>
        /// <param name="graphicsDevice">Graphicsdevice the spritebatch should use</param>
        /// <param name="task">What we want to render to the texture</param>
        /// <param name="dimensions">Size of the texture</param>
        /// <param name="renderTarget">The target we want to render to</param>
        public static void Render(GameTime gameTime, RenderTask task, Vector2 dimensions, out RenderTarget2D renderTarget)
        {
            if (mSpriteBatch == null)
                mSpriteBatch = new SpriteBatch(GameEnvironment.Instance.GraphicsDevice);

            renderTarget = new RenderTarget2D(mSpriteBatch.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y, false,
                mSpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            mSpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            mSpriteBatch.Begin();
            task(gameTime, mSpriteBatch);
            mSpriteBatch.GraphicsDevice.Clear(Color.Transparent);
            mSpriteBatch.End();
        }
    }
}