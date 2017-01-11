using Microsoft.Xna.Framework;

namespace MeesGame
{
    class KeyTile : FloorTile
    {
        public KeyTile(int layer = 0, string id = "") : base(TileType.Key, layer, id)
        {
            secondarySpriteColor = Color.Blue;
        }

        public override void UpdateGraphics()
        {
            if (IsVisited)
                secondarySprite = null;
        }

        public override void EnterTile(Player player)
        {
            player.Inventory.Items.Add(new InventoryKey());
            GameEnvironment.AssetManager.PlaySound("key_pickup");
            base.EnterTile(player);
        }
    }
}
