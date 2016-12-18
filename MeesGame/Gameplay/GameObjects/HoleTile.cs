namespace MeesGame
{
    class HoleTile : FloorTile
    {
        public HoleTile(int layer = 0, string id = "") : base("hole", TileType.Hole, layer, id)    //Change the name of the sprite once there is one
        {

        }

        public bool FallInHole(Player player)
        {
            if (player.Position == this.position)
                return true;
            else
                return false;
        }
    }
}
