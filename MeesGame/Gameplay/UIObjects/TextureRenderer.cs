using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    public class TextureRenderer
    {
        public delegate void RenderTask(GameTime gameTime, SpriteBatch spriteBatch);

        //spritebatch used for rendering our texture. Needs to be instantiate for each seperate texture,
        //otherwise we would have to end the previous draw
        public SpriteBatch mSpriteBatch;

        /// <summary>
        /// render the task to a texture
        /// </summary>
        /// <param name="gameTime">time used for the rendertask</param>
        /// <param name="graphicsDevice">graphicsdevice the spritebatch should use</param>
        /// <param name="task">what we want to render to the texture</param>
        /// <param name="dimensions">size of the texture</param>
        /// <param name="renderTarget">the target we want to render to</param>
        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, RenderTask task, Vector2 dimensions, out RenderTarget2D renderTarget)
        {
            if (mSpriteBatch == null)
                mSpriteBatch = new SpriteBatch(graphicsDevice);

            RenderTargetBinding[] oldTargets = graphicsDevice.GetRenderTargets();
            renderTarget = new RenderTarget2D(mSpriteBatch.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y, false,
                mSpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            mSpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            mSpriteBatch.GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();
            task(gameTime, mSpriteBatch);
            mSpriteBatch.End();
            mSpriteBatch.GraphicsDevice.SetRenderTargets(oldTargets);
            }
        }
    }

