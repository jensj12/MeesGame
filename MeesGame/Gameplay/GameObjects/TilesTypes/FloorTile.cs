namespace MeesGame
{
    class FloorTile : Tile
    {
        protected FloorTile(TileType tt = TileType.Floor, int layer = 0, string id = "") : base(tt, layer, id)
        {
        }

        public FloorTile(int layer = 0, string id = "") : base(TileType.Floor, layer, id)
        {
        }

        public override bool CanPlayerMoveHere(ITileFieldPlayer player)
        {
            //A player is allowed to move onto floors
            return true;
        }
    }
}
