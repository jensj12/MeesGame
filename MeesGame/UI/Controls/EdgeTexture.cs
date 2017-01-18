using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class EdgeTexture : Background
    {
        int edgeThickness;

        public EdgeTexture(int? edgeThickness = null, Color? color = null) : base(color: color)
        {
            this.edgeThickness = edgeThickness ?? DefaultUIValues.Default.DefaultEdgeThickness;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            if (CurrentTexture == null || CurrentTexture.Bounds.Size != CurrentDimensions)
            {
                CurrentTexture?.Dispose();
                CurrentTexture = Utility.EdgeTexture(CurrentDimensions.X, CurrentDimensions.Y, edgeThickness);
            }
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
        }
    }
}
