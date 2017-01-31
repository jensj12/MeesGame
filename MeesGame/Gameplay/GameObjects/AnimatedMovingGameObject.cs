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

        private void UseNextAnimationSprite(GameTime gameTime)
        {
            if (IsSliding)
                Sprite.SheetRowIndex++;
            else
                Sprite.SheetColIndex++;
            lastAnimationTime = gameTime.TotalGameTime;
        }

        private bool isTimeForNextAnimation(GameTime gameTime)
        {
            return (gameTime.TotalGameTime - lastAnimationTime) >= timeBetweenAnimations;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMoving)
            {
                if (isTimeForNextAnimation(gameTime))
                    UseNextAnimationSprite(gameTime);
            }
            else
                Sprite.SheetColIndex = 0;
        }

        protected virtual bool IsSliding
        {
            get
            {
                return false;
            }
        }
    }
}
