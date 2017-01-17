using System;
using Microsoft.Xna.Framework;

namespace MeesGame
{
    /// <summary>
    /// A SmoothlyMovingGameObject that is animated while moving. When standing still it will display the sprite at index 0.
    /// </summary>
    public class AnimatedMovingGameObject : SmoothlyMovingGameObject
    {
        /// <summary>
        /// The time the last animation sprite was started
        /// </summary>
        private TimeSpan lastAnimationTime;

        /// <summary>
        /// The time between 2 animation frames
        /// </summary>
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
                    Sprite.SheetColIndex++;
                    lastAnimationTime = gameTime.TotalGameTime;
                }
            }
            else
            {
                Sprite.SheetIndex = Sprite.SheetRowIndex * Sprite.NumberColumns;
            }
        }
    }
}
