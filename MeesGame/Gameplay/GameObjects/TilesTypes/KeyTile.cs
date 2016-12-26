using Microsoft.Xna.Framework;

namespace MeesGame
{
    class KeyTile : FloorTile
    {
        new const string defaultAssetName = "keyOverlay";

        public KeyTile(int layer = 0, string id = "") : base(FloorTile.defaultAssetName, TileType.Key, layer, id)
        {
            secondarySprite = new SpriteSheet(defaultAssetName);
            secondarySpriteColor = Color.Blue;
        }

        public override InventoryItem GetItem()
        {
            InventoryItem key = new InventoryKey();
            return key;
        }

        public override void UpdateGraphics()
        {
            if (isVisited)
                secondarySprite = null;
        }

        new public static string[] GetDefaultAssetNames()
        {
            string[] parentDefaultAssetNames = FloorTile.GetDefaultAssetNames();
            string[] defaultAssetNames = new string[parentDefaultAssetNames.Length + 1];
            parentDefaultAssetNames.CopyTo(defaultAssetNames, 0);
            defaultAssetNames[parentDefaultAssetNames.Length] = defaultAssetName;
            return defaultAssetNames;
        }

        //TODO: if key has been picked up, keytile changes into floortile.
    }
}
