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

        public override void UpdateGraphics()
        {
            if (IsVisited)
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

        public override void EnterTile(Player player)
        {
            player.Inventory.Items.Add(new InventoryKey());
            base.EnterTile(player);
        }
    }
}
