using MeesGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIObjects
{
    class ImageView : UIObject
    {
        int imageOffset;

        SpriteSheet image;

        public ImageView(Vector2 location, Vector2 dimensions, string textureName = "", int imageOffset = 5) : base(location, dimensions)
        {
            this.imageOffset = imageOffset;
            image = new SpriteSheet(textureName);
        }

        public override void DrawTask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawTask(gameTime, spriteBatch);
            image.Draw(spriteBatch, new Vector2((int)imageOffset, (int)imageOffset), Vector2.Zero, (int)Dimensions.X - imageOffset * 2, (int)Dimensions.Y - imageOffset * 2);
        }
    }
}