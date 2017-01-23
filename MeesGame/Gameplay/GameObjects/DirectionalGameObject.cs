namespace MeesGame
{
    public class DirectionalGameObject : SpriteGameObject
    {
        private Direction direction = Direction.NORTH;

        public DirectionalGameObject(string assetName, int layer = 0, string id = "", int sheetIndex = 0) : base(assetName, layer, id, sheetIndex)
        {
        }

        public Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                Sprite.SheetRowIndex = direction.ToSheetIndex();
            }
        }
    }
}
