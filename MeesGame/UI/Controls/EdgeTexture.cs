using MeesGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MeesGame
{
    class EdgeTexture : Background
    {
        int edgeThickness;

        protected Color? edgeColor;
        protected Color? innerColor;

        public EdgeTexture(int? edgeThickness = null, Color? edgeColor = null, Color? innerColor = null, Color? color = null) : base(color: color)
        {
            this.edgeThickness = edgeThickness ?? DefaultUIValues.Default.EdgeThickness;
            this.edgeColor = edgeColor;
            this.innerColor = innerColor;
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 anchorPoint)
        {
            CurrentTexture?.Dispose();
            CurrentTexture = Utility.EdgeTexture(CachedDimensions.X, CachedDimensions.Y, edgeThickness, edgeColor, innerColor);
            base.DrawTask(gameTime, spriteBatch, anchorPoint);
        }

        public Color? EdgeColor
        {
            get { return edgeColor; }
            set
            {
                edgeColor = value;
                Invalidate();
            }
        }
    }
}
