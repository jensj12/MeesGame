namespace MeesGame
{
    class HoleTile : FloorTile
    {
        new const string defaultAssetName = "hole";

        public HoleTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.Hole, layer, id)    //Change the name of the sprite once there is one
        {

        }

        new public static string[] GetDefaultAssetNames()
        {
            string[] parentDefaultAssetNames = FloorTile.GetDefaultAssetNames();
            string[] defaultAssetNames = new string[parentDefaultAssetNames.Length + 1];
            parentDefaultAssetNames.CopyTo(defaultAssetNames, 0);
            defaultAssetNames[parentDefaultAssetNames.Length] = defaultAssetName;
            return defaultAssetNames;
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
