namespace MeesGame
{
    class EndTile : FloorTile
    {
        public EndTile(int layer = 0, string id = "") : base(TileType.End, layer, id)
        {
        }

        public override void EnterCenterOfTile(Player player)
        {
            player.Win();
        }
    }
}
