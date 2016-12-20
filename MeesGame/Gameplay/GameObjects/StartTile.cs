using Microsoft.Xna.Framework;

namespace MeesGame
{
    class StartTile : FloorTile
    {
        new public const string defaultAssetName = "floorTile";
        
        public StartTile(int layer = 0, string id = "") : base(defaultAssetName, TileType.Start, layer, id)  //Change the name of the sprite once there is one
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
    }
}
