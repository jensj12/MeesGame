using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class TextureRenderer
    {
        public delegate void RenderTask(GameTime gameTime, SpriteBatch spriteBatch);

        //Spritebatch used for rendering our texture. Needs to be instantiate for each separate texture,
        //otherwise we would have to end the previous draw
        public SpriteBatch mSpriteBatch;

        /// <summary>
        /// Render the task to a texture
        /// </summary>
        /// <param name="gameTime">Time used for the rendertask</param>
        /// <param name="graphicsDevice">Graphicsdevice the spritebatch should use</param>
        /// <param name="task">What we want to render to the texture</param>
        /// <param name="dimensions">Size of the texture</param>
        /// <param name="renderTarget">The target we want to render to</param>
        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, RenderTask task, Vector2 dimensions, out RenderTarget2D renderTarget)
        {
            if (mSpriteBatch == null)
                mSpriteBatch = new SpriteBatch(graphicsDevice);

            RenderTargetBinding[] oldTargets = graphicsDevice.GetRenderTargets();
            renderTarget = new RenderTarget2D(mSpriteBatch.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y, false,
                mSpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            mSpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            mSpriteBatch.Begin();
            task(gameTime, mSpriteBatch);
            mSpriteBatch.GraphicsDevice.Clear(Color.Transparent);
            mSpriteBatch.End();
            mSpriteBatch.GraphicsDevice.SetRenderTargets(oldTargets);
        }
    }
}