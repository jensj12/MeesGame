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

        public override void EnterCenterOfTile(Player player)
        {
            player.Lose();
        }
    }
}
