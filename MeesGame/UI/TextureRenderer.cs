using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    /// <summary>
    /// Renders a RenderTask to a Texture2d.
    /// </summary>
    public static class TextureRenderer
    {
        public delegate void RenderTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint);

        /// <summary>
        /// Render the task to the texture.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphicsDevice">Graphicsdevice the spritebatch should use.</param>
        /// <param name="task">The RenderTaskt that is to be rendered to a texture.</param>
        /// <param name="renderTarget">The target to render to.</param>
        public static void Render(GameTime gameTime, RenderTask task, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);

            spriteBatch.GraphicsDevice.Clear(Color.Transparent);

            task(gameTime, spriteBatch, Vector2.Zero);

            spriteBatch.End();
            UIComponent.BeginUISpriteBatch(spriteBatch) ;
        }
    }
}
