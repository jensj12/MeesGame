using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class TextureGenerator
    {
        public delegate void RenderTask(GameTime gameTime, SpriteBatch spriteBatch);

        //for performance reasons we keep the texture in memory and just overwrite it when it is needed
        private RenderTarget2D renderTarget;
        private SpriteBatch tmpSpriteBatch;
        private Color backgroundColor;

        public TextureGenerator(GraphicsDevice device, int width, int height, Color backgroundColor)
        {
            this.backgroundColor = backgroundColor;
            renderTarget = new RenderTarget2D(device, width, height, false,
                device.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            tmpSpriteBatch = new SpriteBatch(device);
        }

        public RenderTarget2D Render(GameTime gameTime, RenderTask task)
        {
            tmpSpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);

            tmpSpriteBatch.GraphicsDevice.Clear(backgroundColor);
            tmpSpriteBatch.Begin();
            task(gameTime, tmpSpriteBatch);
            tmpSpriteBatch.End();

            // Drop the render target
            tmpSpriteBatch.GraphicsDevice.SetRenderTarget(null);

            return renderTarget;
        }
    }
}
