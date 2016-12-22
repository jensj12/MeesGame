using System.Collections.Generic;

namespace MeesGame
{
    public class RotatableGameObject : SpriteGameObject
    {
        private Dictionary<Direction, SpriteSheet> sprites;
        private Direction facingDirection;
        protected Direction FacingDirection
        {
            get
            {
                return facingDirection;
            }
            set
            {
                facingDirection = value;
                sprites.TryGetValue(facingDirection, out sprite);
            }
        }
        public RotatableGameObject(Dictionary<Direction, SpriteSheet> sprites, string assetName, int layer = 0, string id = "", int sheetIndex = 0) : base(assetName, layer, id, sheetIndex)
        {
            this.sprites = sprites;
            FacingDirection = Direction.NORTH;
        }
    }
}
