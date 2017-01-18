namespace MeesGame
{
    class EndTile : WallTile
    {
        public EndTile(int layer = 0, string id = "") : base(TileType.End, layer, id)
        {
        }

        public override void EnterCenterOfTile(ITileFieldPlayer player)
        {
            player.Win();
        }

        public override bool CanPlayerMoveHere(ITileFieldPlayer player)
        {
            return true;
        }

        public override void UpdateGraphicsToMatchSurroundings()
        {
            int x = Location.X;
            TileField tileField = Parent as TileField;

            if (x == 0 || x == tileField.Columns - 1)
            {
                sprite = new SpriteSheet("verticalEnd");
            }
            else
            {
                sprite = new SpriteSheet("horizontalEnd");
            }
        }
    }
}
