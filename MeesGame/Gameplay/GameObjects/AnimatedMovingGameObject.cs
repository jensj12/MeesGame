using System;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    /// <summary>
    /// A SmoothlyMovingGameObject that is animated while moving. When standing still it will display the sprite at index 0.
    /// </summary>
    public class AnimatedMovingGameObject : SmoothlyMovingGameObject
    {
        private TimeSpan lastAnimationTime;
        private TimeSpan timeBetweenAnimations;

        public AnimatedMovingGameObject(IDiscreteField field, TimeSpan travelTime, string assetName, int layer = 0, string id = "", int sheetIndex = 0) : base(field, travelTime, assetName, layer, id, sheetIndex)
        {
            timeBetweenAnimations = TimeSpan.FromSeconds(travelTime.TotalSeconds / Sprite.NumberColumns);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMoving)
            {
                if (gameTime.TotalGameTime - lastAnimationTime >= timeBetweenAnimations)
                {
                    Sprite.SheetIndex = Sprite.SpriteRowIndex * Sprite.NumberColumns + ((Sprite.SheetIndex + 1) % Sprite.NumberColumns);
                    lastAnimationTime = gameTime.TotalGameTime;
                }
            }
            else
            {
                Sprite.SheetIndex = Sprite.SpriteRowIndex;
            }
        }
    }
}
