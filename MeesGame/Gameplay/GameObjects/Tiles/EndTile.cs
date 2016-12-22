namespace MeesGame
{
    class EndTile : FloorTile
    {
        new const string defaultAssetName = "money";

        public EndTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.End, layer, id)
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
            player.Win();
        }
    }
}
