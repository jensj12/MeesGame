namespace MeesGame
{
    class HoleTile : FloorTile
    {
        public HoleTile(int layer = 0, string id = "") : base(TileType.Hole, layer, id)
        {
        }

        public override void EnterCenterOfTile(ITileFieldPlayer player)
        {
            player.Lose();
        }
    }
}
