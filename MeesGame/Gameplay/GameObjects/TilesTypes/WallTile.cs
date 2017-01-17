namespace MeesGame
{
    class WallTile : Tile
    {
        protected WallTile(TileType tt = TileType.Wall, int layer = 0, string id = "") : base(tt, layer, id)
        {
            obstructsVision = true;
        }

        public WallTile(int layer = 0, string id = "") : base(TileType.Wall, layer, id)
        {
            obstructsVision = true;
        }

        public override bool CanPlayerMoveHere(ITileFieldPlayer player)
        {
            //A player can never walk onto a wall
            return false;
        }

        public override bool IsActionForbiddenFromHere(ITileFieldPlayer player, CharacterAction action)
        {
            //A player should never even be on a Wall Tile
            return true;
        }

        public override void UpdateGraphicsToMatchSurroundings()
        {
            int sheetIndex = 0;
            int x = Location.X;
            int y = Location.Y;
            TileField tileField = Parent as TileField;
            if (tileField.GetTile(x, y - 1) is WallTile) sheetIndex += 1;
            if (tileField.GetTile(x + 1, y) is WallTile) sheetIndex += 2;
            if (tileField.GetTile(x, y + 1) is WallTile) sheetIndex += 4;
            if (tileField.GetTile(x - 1, y) is WallTile) sheetIndex += 8;
            sprite = new SpriteSheet(GetAssetNamesFromTileType(TileType.Wall)[0], sheetIndex);
        }

        public override void UpdateGraphics()
        {
        }

        public override bool StopsSliding
        {
            get
            {
                return true;
            }
        }
    }
}
