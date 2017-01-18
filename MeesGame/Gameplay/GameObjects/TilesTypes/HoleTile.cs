namespace MeesGame
{
    class HoleTile : FloorTile
    {
        public HoleTile(int layer = 0, string id = "") : base(TileType.Hole, layer, id)
        {
        }

        protected HoleTile(TileType tt, int layer = 0, string id = "") : base(tt, layer, id)
        {
        }

        public override void EnterCenterOfTile(ITileFieldPlayer player)
        {
            player.Lose();
        }
    }
}
